using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;

using Microsoft.Band;
using Microsoft.Band.Tiles;
using Microsoft.Band.Tiles.Pages;

using Roboworks.Hue.Entities;
using Roboworks.Band.Common;

namespace Roboworks.Band.Tiles.PhilipsHue.Services
{
    public interface IBandService
    {
        event EventHandler<BulbEventArgs> BulbBrightnessDownRequested;
        event EventHandler<BulbEventArgs> BulbBrightnessUpRequested;
        event EventHandler<BulbEventArgs> BulbSwitchRequested;

        Task Initialize();

        Task<BandInfo> BandInfoGet();

        Task<BandTileInfo[]> BandTileInfosGet();

        Task BandTileCreate(HueLightBulb[] hueLightBulbs);

        Task BandTileDelete();
    }

    public class BandService : IBandService, IDisposable
    {
        private const string TileIconUri = "ms-appx:///Assets/TileIcons/LightBulb.png";
        private const string TileIconSmallUri = "ms-appx:///Assets/TileIcons/LightBulb_Small.png";

        private readonly HueLightBulbPage _hueLightBulbPage = new HueLightBulbPage();

        private IBandInfo _pairedBand = null;
        private IBandClient _bandClient = null;

        private Dictionary<Guid, string> _pageBulbIds = new Dictionary<Guid, string>();

#region Events

        public event EventHandler<BulbEventArgs> BulbBrightnessDownRequested;
        private void OnBulbBrightnessDownRequested(BulbEventArgs args)
        {
            this.BulbBrightnessDownRequested?.Invoke(this, args);
        }

        public event EventHandler<BulbEventArgs> BulbBrightnessUpRequested;
        private void OnBulbBrightnessUpRequested(BulbEventArgs args)
        {
            this.BulbBrightnessUpRequested?.Invoke(this, args);
        }

        public event EventHandler<BulbEventArgs> BulbSwitchRequested;
        private void OnBulbSwitchRequested(BulbEventArgs args)
        {
            this.BulbSwitchRequested?.Invoke(this, args);
        }

#endregion
        
#region Public Methods

        public async Task Initialize()
        {
            if (this._pairedBand == null)
            {
                var pairedBands = await BandClientManager.Instance.GetBandsAsync();
                this._pairedBand = pairedBands.FirstOrDefault();
            }

            if (this._bandClient == null)
            {
                this._bandClient = await BandClientManager.Instance.ConnectAsync(this._pairedBand);
                this._bandClient.TileManager.TileButtonPressed += this.TileManager_TileButtonPressed;
            }
        }

        public async Task<BandInfo> BandInfoGet()
        {
            this.BandClientCheck();

            string[] versions;
            int remainingTileCapacity;

            versions = await
                Task.WhenAll(
                    this._bandClient.GetFirmwareVersionAsync(),
                    this._bandClient.GetHardwareVersionAsync()
                );

            remainingTileCapacity = await this._bandClient.TileManager.GetRemainingTileCapacityAsync();

            return
                new BandInfo(
                    this._pairedBand.Name,
                    versions[0],
                    versions[1],
                    remainingTileCapacity
                );
        }

        public async Task<BandTileInfo[]> BandTileInfosGet()
        {
            this.BandClientCheck();
            
            var tiles = await this._bandClient.TileManager.GetTilesAsync();
            return tiles.Select(tile => new BandTileInfo(tile)).ToArray();
        }

        public async Task BandTileCreate(HueLightBulb[] hueLightBulbs)
        {
            this.BandClientCheck();
            
            var loadIconMethod = this._hueLightBulbPage.LoadIconMethod;
            
            var tile = 
                new BandTile(Guid.NewGuid())
                {
                    IsBadgingEnabled = true,
                    Name = "Hue",
                    SmallIcon = await loadIconMethod.Invoke(BandService.TileIconSmallUri),
                    TileIcon = await loadIconMethod.Invoke(BandService.TileIconUri)
                };
            
            var pageDataList = new List<PageData>(hueLightBulbs.Length);

            this._pageBulbIds.Clear();

            foreach (var hueLightBulb in hueLightBulbs)
            {
                var pageData = new PageData(Guid.NewGuid(), tile.PageLayouts.Count);
                this.PageDataPopulate(pageData, hueLightBulb);

                this._pageBulbIds.Add(pageData.PageId, hueLightBulb.Id);
                
                tile.PageLayouts.Add(this._hueLightBulbPage.Layout);
                pageDataList.Add(pageData);
            }

            await this._bandClient.TileManager.AddTileAsync(tile);
            await this._bandClient.TileManager.SetPagesAsync(tile.TileId, pageDataList);

            await this._bandClient.TileManager.StartReadingsAsync();
        }

        public async Task BandTileDelete()
        {
            await this._bandClient.TileManager.StopReadingsAsync();

            var tiles = await this._bandClient.TileManager.GetTilesAsync();

            foreach(var tile in tiles)
            {
                await this._bandClient.TileManager.RemoveTileAsync(tile);
            }
        }

#endregion

#region Private Methods

        //private async Task<BandIcon> TileIconCreate(string fileUri, int width, int height)
        //{
        //    var bitmap = new WriteableBitmap(width, height);
        //    var imageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(fileUri));

        //    using (var fileStream = await imageFile.OpenAsync(FileAccessMode.Read))
        //    {    
        //        await bitmap.SetSourceAsync(fileStream);
        //    }

        //    return bitmap.ToBandIcon();
        //}

        private void BandClientCheck()
        {
            if (this._pairedBand == null)
            {
                throw new InvalidOperationException("Paired band has not been found.");
            }

            if (this._bandClient == null)
            {
                throw new InvalidOperationException("Band client has not been connected to paired band.");
            }
        }

        private void PageDataPopulate(PageData pageData, HueLightBulb hueLightBulb)
        {
            pageData.Values.Add(
                new TextBlockData(
                    this._hueLightBulbPage.BulbName.ElementId.Value, 
                    string.Format(this._hueLightBulbPage.BulbNameData.Text, hueLightBulb.Name)
                )
            );

            pageData.Values.Add(
                new TextBlockData(
                    this._hueLightBulbPage.Brightness.ElementId.Value, 
                    string.Format(
                        this._hueLightBulbPage.BrightnessData.Text, 
                        hueLightBulb.Brightness.ToPercentage()
                    )
                )
            );

            pageData.Values.Add(this._hueLightBulbPage.DimButtonData);
            pageData.Values.Add(this._hueLightBulbPage.OnOffButtonData);
            pageData.Values.Add(this._hueLightBulbPage.BrightenButtonData);
        }
        
        private async void TileManager_TileButtonPressed(
            object sender, 
            BandTileEventArgs<IBandTileButtonPressedEvent> e)
        {
            await DispatcherHelper.Invoke(
                () => this.TileButtonPressedProcess(e.TileEvent.PageId, e.TileEvent.ElementId)
            );
        }
        
        private void TileButtonPressedProcess(Guid pageId, ushort elementId)
        {
            if (this._pageBulbIds.ContainsKey(pageId))
            {
                var eventArgs = new BulbEventArgs(this._pageBulbIds[pageId]);

                if (this._hueLightBulbPage.OnOffButton.ElementId == elementId)
                {
                    this.OnBulbSwitchRequested(eventArgs);
                }
                else if (this._hueLightBulbPage.BrightenButton.ElementId == elementId)
                {
                    this.OnBulbBrightnessUpRequested(eventArgs);
                }
                else if (this._hueLightBulbPage.DimButtonData.ElementId == elementId)
                {
                    this.OnBulbBrightnessDownRequested(eventArgs);
                }
            }
        }

#endregion

#region IDisposable pattern

        private bool _isDisposed = false;

        public void Dispose()
        { 
            this.Dispose(true);
            GC.SuppressFinalize(this);           
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!this._isDisposed)
            {
                if (isDisposing)
                {
                    if (this._bandClient != null)
                    {
                        //this._bandClient.TileManager.StopReadingsAsync().Wait();
                        this._bandClient.TileManager.TileButtonPressed -= this.TileManager_TileButtonPressed;

                        this._bandClient.Dispose();
                    }
                }
                
                this._isDisposed = true;
            }
        }

        ~BandService()
        {
            this.Dispose(false);
        }

#endregion

    }

    public class BulbEventArgs : EventArgs
    {
        public string BulbId { get; }

        public BulbEventArgs(string bulbId)
        {
            if (bulbId == null)
            {
                throw new ArgumentNullException(nameof(bulbId));
            }

            this.BulbId = bulbId;
        }
    }

    public class BandInfo
    {
        public string Name { get; }

        public string FirmwareVersion { get; }

        public string HardwareVersion { get; }

        public int RemainingTileCapacity { get; }

        public BandInfo(
            string name,
            string firmwareVersion,
            string hardwareVersion,
            int remainingTileCapacity)
        {
            this.Name = name;
            this.FirmwareVersion = firmwareVersion;
            this.HardwareVersion = hardwareVersion;
            this.RemainingTileCapacity = remainingTileCapacity;
        }
    }

    public class BandTileInfo
    {
        private string Name { get; }

        private Guid TileId { get; }

        public BandTileInfo(Microsoft.Band.Tiles.BandTile data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.Name = data.Name;
            this.TileId = data.TileId;
        }
    }
}
