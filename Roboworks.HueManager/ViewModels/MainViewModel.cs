using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;

using Prism.Mvvm;
using Prism.Commands;

using Roboworks.HueManager.Services;
using Roboworks.Hue;

namespace Roboworks.HueManager.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private const double BrightnessStep = 0.1d;

        private readonly IBandService _bandService;
        private readonly IHueService _hueService;

        private bool _isTileManagementEnabled = true;
        private bool _isTileRequestInProgress = false;

#region Properties

        private bool _isBusy = true;
        public bool IsBusy
        {
            get
            {
                return this._isBusy;
            }
            set
            {
                this.SetProperty(ref this._isBusy, value);
            }
        }

        private BandInfo _bandInfo = null;
        public BandInfo BandInfo
        {
            get
            {
                return this._bandInfo;
            }
            set
            {
                this.SetProperty(ref this._bandInfo, value);
            }
        }

        private ReadOnlyCollection<BandTileInfo> _bandTiles = null;
        public ReadOnlyCollection<BandTileInfo> BandTiles
        {
            get
            {
                return this._bandTiles;
            }
            set
            {
                this.SetProperty(ref this._bandTiles, value);
            }
        }

        private ReadOnlyCollection<HueLightBulb> _hueLightBulbs;
        public ReadOnlyCollection<HueLightBulb> HueLightBulbs
        {
            get
            {
                return this._hueLightBulbs;
            }
            set
            {
                this.SetProperty(ref this._hueLightBulbs, value);
            }
        }

#endregion

#region Commands

        private DelegateCommand _tileAddCommand;
        public ICommand TileAddCommand => this._tileAddCommand;

        private DelegateCommand _tilesRemoveCommand;
        public ICommand TilesRemoveCommand => this._tilesRemoveCommand;

#endregion
        
        public MainViewModel(IBandService bandService, IHueService hueService)
        {
            if (bandService == null)
            {
                new ArgumentNullException(nameof(bandService));
            }

            if (hueService == null)
            {
                new ArgumentNullException(nameof(hueService));
            }

            this._bandService = bandService;
            this._hueService = hueService;

            this._bandService.BulbSwitchRequested += this.BandService_BulbSwitchRequested;
            this._bandService.BulbBrightnessDownRequested += this.BandService_BulbBrightnessDownRequested;
            this._bandService.BulbBrightnessUpRequested += this.BandService_BulbBrightnessUpRequested;

            this._tileAddCommand = 
                new DelegateCommand(
                    this.TileAddCommand_Executed, 
                    () => this._isTileManagementEnabled
                );

            this._tilesRemoveCommand = 
                new DelegateCommand(
                    this.TilesRemoveCommand_Executed,
                    () => this._isTileManagementEnabled
                );
        }
        
#region Public Methods

        public async Task Initialize()
        {
            await this._bandService.Initialize();

            this.BandInfo = await this._bandService.BandInfoGet();

            await this.HueLightBulbsRefresh();
            
            this.IsBusy = false;
        }

#endregion

#region Private Methods

        private async Task HueLightBulbsRefresh()
        {
            this.HueLightBulbs = (await this._hueService.LightBulbsGet()).ToList().AsReadOnly();
        }

        private async Task BandTilesRefresh()
        {
            this.BandTiles = (await this._bandService.BandTileInfosGet()).ToList().AsReadOnly();
        }

        private void TileManagementCommandsUpdate(bool isEnabled)
        {
            this._isTileManagementEnabled = isEnabled;

            this._tileAddCommand.RaiseCanExecuteChanged();
            this._tilesRemoveCommand.RaiseCanExecuteChanged();
        }

        private async void TileAddCommand_Executed()
        {
            this.TileManagementCommandsUpdate(false);

            try
            {
                await this._bandService.BandTileCreate(this.HueLightBulbs.ToArray());
                await this.BandTilesRefresh();
            }
            catch(Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
            
            this.TileManagementCommandsUpdate(true);
        }

        private async void TilesRemoveCommand_Executed()
        {
            this.TileManagementCommandsUpdate(false);

            try
            {
                await this._bandService.BandTileDelete();
                await this.BandTilesRefresh();
            }
            catch(Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }

            this.TileManagementCommandsUpdate(true);
        }

        public async Task LightBulbManipulate(string lightBulbId, Func<HueLightBulb, Task> bulbManipulation)
        {
            if (!this._isTileRequestInProgress)
            {
                this._isTileRequestInProgress = true;

                var hueLightBulb = 
                    this.HueLightBulbs
                        .Where(item => item.Id == lightBulbId)
                        .FirstOrDefault();

                if (hueLightBulb != null)
                {
                    await bulbManipulation.Invoke(hueLightBulb);

                    await this.HueLightBulbsRefresh();
                }

                this._isTileRequestInProgress = false;
            }
        }

        private async void BandService_BulbSwitchRequested(object sender, BulbEventArgs e)
        {
            await 
                this.LightBulbManipulate(
                    e.BulbId,
                    async (hueLightBulb) =>
                        await this._hueService.LightBulbIsOnSet(e.BulbId, !hueLightBulb.IsOn)
                );
        }

        private async void BandService_BulbBrightnessDownRequested(object sender, BulbEventArgs e)
        {
            await 
                this.LightBulbManipulate(
                    e.BulbId,
                    async (hueLightBulb) =>
                        await this._hueService.LightBulbBrightnessSet(
                            e.BulbId, 
                            Math.Max(0d, hueLightBulb.Brightness - MainViewModel.BrightnessStep)
                        )
                );
        }

        private async void BandService_BulbBrightnessUpRequested(object sender, BulbEventArgs e)
        {
            await 
                this.LightBulbManipulate(
                    e.BulbId,
                    async (hueLightBulb) =>
                        await this._hueService.LightBulbBrightnessSet(
                            e.BulbId, 
                            Math.Min(1d, hueLightBulb.Brightness + MainViewModel.BrightnessStep)
                        )
                );
        }

#endregion

    }
}
