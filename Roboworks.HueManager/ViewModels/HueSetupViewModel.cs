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
using Roboworks.HueManager.Services;

namespace Roboworks.HueManager.ViewModels
{
    using HueEntities = Roboworks.Hue.Entities;

    public enum HueSetupViewModelState
    {
        Disconnected = 1,
        Connecting,
        Connected,
        Disconnecting
    }

    public class HueSetupViewModel : BindableBase, INavigationAware
    {
        private readonly IHueServiceProvider _hueServiceProvider;
        private readonly IBandService _bandService;
        private readonly ISettingsProvider _settingsProvider;

        private IHueService _hueService = null;
        
#region Properties

        private HueSetupViewModelState? _state = null;
        public HueSetupViewModelState? State
        {
            get
            {
                return this._state;
            }
            set
            {
                if (this._state != value)
                {
                    this._state = value;
                }
            }
        }
        
        private string _ipAddress = null;
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

#region Events

        public event EventHandler StateChanged;
        private void OnStateChanged(EventArgs args)
        {
            this.StateChanged?.Invoke(this, args);
        }

#endregion

        public HueSetupViewModel(
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
            this.State = HueSetupViewModelState.Connecting;

            Exception error = null;

            try
            {
                var ipAddress = this.IpAddress;

                var hueApiUser = await 
                    this._hueServiceProvider.HueApiUserCreate(ipAddress, Constants.AppName);

                this._settingsProvider.HueBridgeIpAddress = ipAddress;
                this._settingsProvider.HueApiUserId = hueApiUser.UserId;

                this._hueService = await this._hueServiceProvider.Connect(ipAddress, hueApiUser.UserId);
                this.HueBridgeInfo = new HueBridgeInfo(this._hueService.HueBridgeInfo);
            }
            catch(Exception ex)
            {
                error = ex;
            }

            if (error != null)
            {
                this.State = HueSetupViewModelState.Disconnected;
            }
            else
            {
                this.State = HueSetupViewModelState.Connected;
            }
        }

        private bool ConnectCommand_CanExecute()
        {
            return true;
            //return !this.IsBusy;
        }

        private async void DisconnectCommand_Executed()
        {
            //this.IsBusy = true;
            
            await 
                this._hueServiceProvider.HueApiUserDelete(
                    this._settingsProvider.HueBridgeIpAddress, 
                    this._settingsProvider.HueApiUserId
                );

            //this.IsBusy = false;
        }

        private bool DisconnectCommand_CanExecute()
        {
            return true;
            //return !this.IsBusy;
        }

        public void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            this.State = HueSetupViewModelState.Disconnected;
            this.OnStateChanged(EventArgs.Empty);

            if (this._hueService != null)
            {
                // Connected state
            }
            else if (
                this._settingsProvider.HueBridgeIpAddress != null && 
                this._settingsProvider.HueApiUserId != null)
            {
                // Connecting state
            }
            else
            {
                // Disconnected state
            }
        }

        public void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
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
}
