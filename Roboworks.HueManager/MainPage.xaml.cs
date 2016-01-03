using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.UI.Core;

using Roboworks.HueManager.ViewModels;
using Roboworks.HueManager.Services;
using Roboworks.Hue;

namespace Roboworks.HueManager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainViewModel _viewModel;

        public MainPage()
        {
            this._viewModel = 
                new MainViewModel(
                    new BandService(this.DispatherInvoke), 
                    new HueService()
                );
            this.DataContext = this._viewModel;

            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await this._viewModel.Initialize();
            }
            catch(Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }
        
        private async Task DispatherInvoke(Action action)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(action));
        }
    }
}
