using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Media.Imaging;

using Microsoft.Band;
using Microsoft.Band.Tiles;
using Microsoft.Band.Tiles.Pages;

using Roboworks.Hue;

namespace Roboworks.HueManager.Services
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
        private readonly Func<Action, Task> _dispatcherInvoke;

        private IBandInfo _pairedBand = null;
        private IBandClient _bandClient = null;

        private Dictionary<Guid, string> _pageBulbIds = new Dictionary<Guid, string>();

        private enum ElementIds : ushort
        {
            BulbName = 1,

            BulbIsOn_Title,
            BulbIsOn_Value,

            BulbBrightness_Title,
            BulbBrightness_Value,

            BulbIsReachable_Title,
            BulbIsReachable_Value,

            BulbBrightnessDown,
            BulbBrightnessUp,
            BulbSwitch,
        }

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

        public BandService(Func<Action, Task> dispatcherInvoke)
        {
            if (dispatcherInvoke == null)
            {
                throw new ArgumentNullException(nameof(dispatcherInvoke));
            }

            this._dispatcherInvoke = dispatcherInvoke;
        }

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
            
            WriteableBitmap smallIconBitmap = new WriteableBitmap(24, 24);
            BandIcon smallIcon = smallIconBitmap.ToBandIcon();

            WriteableBitmap tileIconBitmap = new WriteableBitmap(48, 48);
            BandIcon tileIcon = tileIconBitmap.ToBandIcon();

            Guid tileGuid = Guid.NewGuid();
            var tile = 
                new BandTile(tileGuid)
                {
                    IsBadgingEnabled = true,
                    Name = "Hue",
                    SmallIcon = smallIcon,
                    TileIcon = tileIcon
                };
            
            var pageDataList = new List<PageData>(hueLightBulbs.Length);
            var pageLayoutIndex = 0;

            this._pageBulbIds.Clear();

            foreach(var hueLightBulb in hueLightBulbs)
            {
                var pageLayout = 
                    new PageLayout(
                        new ScrollFlowPanel()
                        {
                            Rect = new PageRect(0, 0, 258, 128),
                            Orientation = FlowPanelOrientation.Vertical,
                            ScrollBarColorSource = ElementColorSource.BandHighlight
                        }
                    );
                var pageId = Guid.NewGuid();
                var pageData = new PageData(pageId, pageLayoutIndex++);

                this._pageBulbIds.Add(pageId, hueLightBulb.Id);

                this.TilePageLayoutPopulate(hueLightBulb, pageLayout, pageData);
                
                tile.PageLayouts.Add(pageLayout);
                pageDataList.Add(pageData);
            }
            
            await this._bandClient.TileManager.AddTileAsync(tile);
            await this._bandClient.TileManager.SetPagesAsync(tileGuid, pageDataList);

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

        private void TilePageLayoutPopulate(
            HueLightBulb hueLightBulb,
            PageLayout pageLayout, 
            PageData pageData)
        {

            pageLayout.Root.Elements.Add(
                new WrappedTextBlock()
                {
                    ElementId = (short)ElementIds.BulbName,
                    Rect = new PageRect(0, 0, 243, 0),
                    AutoHeight = true,
                    ColorSource = ElementColorSource.BandHighlight,
                    Font = WrappedTextBlockFont.Small,
                    HorizontalAlignment = HorizontalAlignment.Left
                }
            );

            pageData.Values.Add(
                new WrappedTextBlockData((short)ElementIds.BulbName, hueLightBulb.Name)
            );

            this.TilePageBulbInfoAdd(hueLightBulb, pageLayout, pageData);
            this.TilePageBulbControlAdd(hueLightBulb, pageLayout, pageData);
        }

        private void TilePageBulbInfoAdd(HueLightBulb hueLightBulb, PageLayout pageLayout, PageData pageData)
        {
            // Is ON

            //this.TilePanelTextAdd(
            //    pageLayout.Root, 
            //    pageData, 
            //    (short)ElementIds.BulbIsOn_Title, 
            //    "Is ON:", 
            //    HorizontalAlignment.Left
            //);

            //this.TilePanelTextAdd(
            //    pageLayout.Root, 
            //    pageData, 
            //    (short)ElementIds.BulbIsOn_Value, 
            //    hueLightBulb.IsOn.ToString(), 
            //    HorizontalAlignment.Right
            //);

            // Brightness

            //this.TilePanelTextAdd(
            //    pageLayout.Root, 
            //    pageData, 
            //    (short)ElementIds.BulbBrightness_Title, 
            //    "Brightness:", 
            //    HorizontalAlignment.Left
            //);

            //this.TilePanelTextAdd(
            //    pageLayout.Root, 
            //    pageData, 
            //    (short)ElementIds.BulbBrightness_Value, 
            //    hueLightBulb.Brightness.ToString(),
            //    HorizontalAlignment.Right
            //);

            // Is reachable

            this.TilePanelTextAdd(
                pageLayout.Root, 
                pageData, 
                (short)ElementIds.BulbIsReachable_Title, 
                "Is reachable:", 
                HorizontalAlignment.Left
            );

            this.TilePanelTextAdd(
                pageLayout.Root, 
                pageData, 
                (short)ElementIds.BulbIsReachable_Value, 
                hueLightBulb.IsReachable.ToString(),
                HorizontalAlignment.Right
            );
        }

        private void TilePageBulbControlAdd(HueLightBulb hueLightBulb, PageLayout pageLayout, PageData pageData)
        {
            var panel = 
                new FlowPanel()
                {
                    //ElementId = elementId,
                    Orientation = FlowPanelOrientation.Horizontal,
                    Rect = new PageRect(0, 0, 243, 48),
                    Margins = new Margins (0, 10, 0, 0)
                };

            panel.Elements.Add(
                new TextButton()
                {
                    ElementId = (short)ElementIds.BulbBrightnessDown,
                    Rect = new PageRect(0, 0, 48, 48),
                    HorizontalAlignment = HorizontalAlignment.Center
                }
            );
            pageData.Values.Add(new TextButtonData((short)ElementIds.BulbBrightnessDown, "<"));

            panel.Elements.Add(
                new TextButton()
                {
                    ElementId = (short)ElementIds.BulbSwitch,
                    Margins = new Margins(10, 0, 10, 0),
                    Rect = new PageRect(0, 0, 127, 48),
                    HorizontalAlignment = HorizontalAlignment.Center
                }
            );
            pageData.Values.Add(new TextButtonData((short)ElementIds.BulbSwitch, "ON / OFF"));

            panel.Elements.Add(
                new TextButton()
                {
                    ElementId = (short)ElementIds.BulbBrightnessUp,
                    Rect = new PageRect(0, 0, 48, 48),
                    HorizontalAlignment = HorizontalAlignment.Center
                }
            );
            pageData.Values.Add(new TextButtonData((short)ElementIds.BulbBrightnessUp, ">"));

            pageLayout.Root.Elements.Add(panel);
        }

        private void TilePanelTextAdd(
            PagePanel pagePanel, 
            PageData pageData, 
            short elementId, 
            string value, 
            HorizontalAlignment horizontalAlignment)
        {
            pagePanel.Elements.Add(
                new WrappedTextBlock()
                {
                    ElementId = elementId,
                    Rect = new PageRect(0, 0, 243, 0),
                    AutoHeight = true,
                    Font = WrappedTextBlockFont.Small,
                    HorizontalAlignment = horizontalAlignment
                }
            );

            pageData.Values.Add(new WrappedTextBlockData(elementId, value));
        }

        private async void TileManager_TileButtonPressed(
            object sender, 
            BandTileEventArgs<IBandTileButtonPressedEvent> e)
        {
            await this._dispatcherInvoke.Invoke(
                () => this.TileButtonPressedProcess(e.TileEvent.PageId, e.TileEvent.ElementId)
            );
        }

        private void TileButtonPressedProcess(Guid pageId, ushort elementId)
        {
            if (this._pageBulbIds.ContainsKey(pageId))
            {
                var eventArgs = new BulbEventArgs(this._pageBulbIds[pageId]);

                switch((ElementIds)elementId)
                {
                    case ElementIds.BulbSwitch:
                        this.OnBulbSwitchRequested(eventArgs);
                        break;

                    case ElementIds.BulbBrightnessUp:
                        this.OnBulbBrightnessUpRequested(eventArgs);
                        break;

                    case ElementIds.BulbBrightnessDown:
                        this.OnBulbBrightnessDownRequested(eventArgs);
                        break;
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
