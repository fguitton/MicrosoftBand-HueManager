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

        public ReadOnlyCollection<MainViewModelTile> Tiles { get; }

#endregion

        public MainViewModel(INavigationService navigationService, ISettingsProvider settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this._settings = settings;

            // TODO: split tiles implementation into separate modules (projects)
            this.Tiles = 
                new MainViewModelTile[]
                {
                    new MainViewModelTile(
                        "Philips Hue", 
                        new DelegateCommand(
                            () => navigationService.Navigate(ViewNames.HueSetup, null),
                            () => true
                        )
                    ),
                    new MainViewModelTile(
                        "Charge Reminder",
                        new DelegateCommand(
                            () => navigationService.Navigate(ViewNames.BandSetup, null),
                            () => true
                        )
                    )
                }.ToList().AsReadOnly();

            this.IsHueConfigured = this._settings.HueBridgeIpAddress != null;
        }

    }

    public class MainViewModelTile
    {
        public string Title { get; }

        public ICommand NavigationCommand { get; }

        public MainViewModelTile(string title, ICommand navigationCommand)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (navigationCommand == null)
            {
                throw new ArgumentNullException(nameof(navigationCommand));
            }

            this.Title = title;
            this.NavigationCommand = navigationCommand;
        }
    }
}
