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
using Prism.Windows.Navigation;

namespace Roboworks.HueManager.ViewModels
{
    public class MainViewModel : BindableBase
    {

        private ISettingsProvider _settings;

#region Properties

        private bool _isHueConfigured = false;
        public bool IsHueConfigured
        {
            get
            {
                return this._isHueConfigured;
            }
            set
            {
                this.SetProperty(ref this._isHueConfigured, value);
            }
        }

#endregion

#region Commands

        private readonly DelegateCommand _hueSetupOpenCommand;
        public ICommand HueSetupOpenCommand => this._hueSetupOpenCommand;

        private readonly DelegateCommand _bandSetupOpenCommand;
        public ICommand BandSetupOpenCommand => this._bandSetupOpenCommand;

#endregion

        public MainViewModel(INavigationService navigationService, ISettingsProvider settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this._settings = settings;

            this._hueSetupOpenCommand = 
                new DelegateCommand(
                    () => navigationService.Navigate(ViewNames.HueSetup, null),
                    () => true
                );

            this._bandSetupOpenCommand =
                new DelegateCommand(
                    () => navigationService.Navigate(ViewNames.BandSetup, null),
                    () => true
                );

            this.IsHueConfigured = this._settings.HueBridgeIpAddress != null;
        }

    }
}
