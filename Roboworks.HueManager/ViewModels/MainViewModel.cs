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
using Roboworks.Band.Common;

namespace Roboworks.HueManager.ViewModels
{
    public class MainViewModel : BindableBase
    {

        private readonly INavigationService _navigationService;

#region Properties

        public ReadOnlyCollection<MainViewModelTile> Tiles { get; }

#endregion

#region Commands

        public ICommand NavigationCommand { get; }

#endregion

        public MainViewModel(INavigationService navigationService, ITileModel[] tileModels)
        {
            if (navigationService == null)
            {
                throw new ArgumentNullException(nameof(navigationService));
            }

            if (tileModels == null)
            {
                throw new ArgumentNullException(nameof(tileModels));
            }

            this._navigationService = navigationService;

            this.NavigationCommand = new DelegateCommand<string>(this.NavigationCommand_Executed);

            this.Tiles =
                tileModels
                    .Select(tileModel => new MainViewModelTile(tileModel.Title, tileModel.ViewName))
                    .ToList()
                    .AsReadOnly();
        }

#region Private Methods

        private void NavigationCommand_Executed(string viewName)
        {
            if (viewName != null)
            {
                this._navigationService.Navigate(viewName, null);
            }
        }

#endregion

    }

    public class MainViewModelTile
    {
        public string Title { get; }

        public string ViewName { get; }

        public MainViewModelTile(string title, string viewName)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (viewName == null)
            {
                throw new ArgumentNullException(nameof(viewName));
            }

            this.Title = title;
            this.ViewName = viewName;
        }
    }
}
