using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Windows.Navigation;

using Roboworks.Hue;
using Roboworks.Band.Tiles.PhilipsHue.Services;
using Roboworks.Band.Common;

namespace Roboworks.Band.Tiles.PhilipsHue.ViewModels
{
    using Hue.Entities;
    using HueEntities = Roboworks.Hue.Entities;

    public class PhilipsHueSetupViewModel : BindableBase, INavigationAware
    {
        private const string PhilipsHue_AppName = "BandEx";
        private const string IpAddressDefault = "192.168.0.0";

        private readonly IHueServiceProvider _hueServiceProvider;
        private readonly IBandService _bandService;
        private readonly ISettingsProvider _settingsProvider;

        private IHueService _hueService = null;
        
#region Properties

        private bool _isBusy = false;
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
        
        private Exception _error = null;
        public Exception Error
        {
            get
            {
                return this._error;
            }
            set
            {
                this.SetProperty(ref this._error, value);
            }
        }

        private string _ipAddress = PhilipsHueSetupViewModel.IpAddressDefault;
        public string IpAddress
        {
            get
            {
                return this._ipAddress;
            }
            set
            {
                this.SetProperty(ref this._ipAddress, value);
            }
        }

        private HueBridgeInfo _hueBridgeInfo = null;
        public HueBridgeInfo HueBridgeInfo
        {
            get
            {
                return this._hueBridgeInfo;
            }
            set
            {
                this.SetProperty(ref this._hueBridgeInfo, value);
            }
        }

#endregion

#region Commands

        private readonly DelegateCommand _connectCommand;
        public ICommand ConnectCommand => this._connectCommand;

        private readonly DelegateCommand _disconnectCommand;
        public ICommand DisconnectCommand => this._disconnectCommand;

#endregion

        public PhilipsHueSetupViewModel(
            IHueServiceProvider hueServiceProvider, 
            IBandService bandService,
            ISettingsProvider settingsProvider)
        {
            if (hueServiceProvider == null)
            {
                throw new ArgumentNullException(nameof(hueServiceProvider));
            }

            if (bandService == null)
            {
                throw new ArgumentNullException(nameof(bandService));
            }

            if (settingsProvider == null)
            {
                throw new ArgumentNullException(nameof(settingsProvider));
            }

            this._hueServiceProvider = hueServiceProvider;
            this._bandService = bandService;
            this._settingsProvider = settingsProvider;

            this._connectCommand = 
                new DelegateCommand(
                    this.ConnectCommand_Executed, 
                    this.ConnectCommand_CanExecute
                );

            this._disconnectCommand =
                new DelegateCommand(
                    this.DisconnectCommand_Executed, 
                    this.DisconnectCommand_CanExecute
                );
        }

#region Private Methods
        
        private async void ConnectCommand_Executed()
        {
            this.IsBusy = true;
            this.Error = null;

            await this.Connect(this.IpAddress);

            this.IsBusy = false;
        }

        private async Task Connect(string ipAddress)
        {
            var hueApiUser = await this.HueApiUserCreateTry(ipAddress);
            if (hueApiUser != null)
            {
                this._settingsProvider.HueBridgeIpAddress = ipAddress;
                this._settingsProvider.HueApiUserId = hueApiUser.UserId;

                this._hueService = await this.HueServiceGetTry(ipAddress, hueApiUser);
                if (this._hueService != null)
                {
                    this.HueBridgeInfo = new HueBridgeInfo(this._hueService.HueBridgeInfo);
                }
            }
        }

        private async Task<HueApiUser> HueApiUserCreateTry(string ipAddress)
        {
            HueApiUser hueApiUser = null;

            try
            {
                hueApiUser = await 
                    this._hueServiceProvider.HueApiUserCreate(
                        ipAddress, 
                        PhilipsHueSetupViewModel.PhilipsHue_AppName
                    );
            }
            catch(Exception ex)
            {
                this.Error = ex;
            }

            return hueApiUser;
        }

        private async Task<IHueService> HueServiceGetTry(string ipAddress, HueApiUser hueApiUser)
        {
            IHueService hueService = null;

            try
            {
                hueService = await this._hueServiceProvider.Connect(ipAddress, hueApiUser.UserId);
            }
            catch(Exception ex)
            {
                this.Error = ex;
            }

            return hueService;
        }

        private bool ConnectCommand_CanExecute()
        {
            return !this.IsBusy;
        }

        private async void DisconnectCommand_Executed()
        {
            this.IsBusy = true;
            this.Error = null;
            
            await 
                this._hueServiceProvider.HueApiUserDelete(
                    this._settingsProvider.HueBridgeIpAddress, 
                    this._settingsProvider.HueApiUserId
                );

            this.IsBusy = false;
        }

        private bool DisconnectCommand_CanExecute()
        {
            return !this.IsBusy;
        }

#endregion

#region INavigationAware

        public void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            if (this._hueService != null)
            {
                // Connected state
            }
            else if (
                this._settingsProvider.HueBridgeIpAddress != null && 
                this._settingsProvider.HueApiUserId != null)
            {
                // Connecting state
                // Start connection
            }
            else
            {
                // Disconnected state
            }
        }

        public void OnNavigatingFrom(
            NavigatingFromEventArgs e, 
            Dictionary<string, object> viewModelState, 
            bool suspending)
        {
        }

#endregion

    }

    public class HueBridgeInfo
    {
        private readonly HueEntities.HueBridgeInfo _hueBridgeInfo;

        public string Id => this._hueBridgeInfo.Id;

        public string Name => this._hueBridgeInfo.Name;

        public string IpAddress => this._hueBridgeInfo.IpAddress;

        public HueBridgeInfo(HueEntities.HueBridgeInfo hueBridgeInfo)
        {
            if (hueBridgeInfo == null)
            {
                throw new ArgumentNullException(nameof(hueBridgeInfo));
            }

            this._hueBridgeInfo = hueBridgeInfo;
        }
    }

    public class HueSetupViewModelStateChangeEventArgs : EventArgs
    {
        public bool IsInitialState { get; }

        public Exception Error { get; }
        
        public HueSetupViewModelStateChangeEventArgs(bool isInitialState, Exception error = null)
        {
            this.IsInitialState = isInitialState;
            this.Error = error;
        }
    }
}
