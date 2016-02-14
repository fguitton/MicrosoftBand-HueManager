using System;
using System.Collections.Generic;
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

using Roboworks.HueManager.ViewModels;

namespace Roboworks.HueManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HueSetupView : Page
    {
        private HueSetupViewModel _viewModel;

        public HueSetupView()
        {
            this.InitializeComponent();
        }

        private void Page_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (this._viewModel != null)
            {
                this._viewModel.StateChanged -= this.ViewModel_StateChanged;
            }

            this._viewModel = sender.DataContext as HueSetupViewModel;

            if (this._viewModel != null)
            {
                this._viewModel.StateChanged += this.ViewModel_StateChanged;
            }
        }

        private void ViewModel_StateChanged(object sender, EventArgs e)
        {
            if (this._viewModel.State.HasValue)
            {
                switch(this._viewModel.State.Value)
                {
                    case HueSetupViewModelState.Disconnected:
                        break;
                }

                VisualStateManager.GoToState(this, "Disconnected", false);
            }
        }
    }
}
