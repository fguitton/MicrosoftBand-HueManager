using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

using Roboworks.Band.Tiles.PhilipsHue.ViewModels;
using Roboworks.Band.Common;
using Roboworks.Band.Tiles.PhilipsHue.Controls;

namespace Roboworks.Band.Tiles.PhilipsHue.Views
{
    public sealed partial class PhilipsHueSetupView : Page
    {
        private enum SubViewType
        {
            Disconnected = 1,
            Connected
        }

        private PhilipsHueSetupViewModel _viewModel = null;

        private SubViewType? _subView = null;
        private bool _isSubViewLoaded = false;

        public PhilipsHueSetupView()
        {
            this.InitializeComponent();
        }

        private void Page_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (this._viewModel != null)
            {
                this._viewModel.StateChanged -= this.ViewModel_StateChanged;
            }

            this._viewModel = args.NewValue as PhilipsHueSetupViewModel;

            if (this._viewModel != null)
            {
                this._viewModel.StateChanged += this.ViewModel_StateChanged;
            }
        }

        private async Task SubViewSet(SubViewType subView, Exception error)
        {
            if (!this._subView.HasValue || this._subView.Value != subView)
            {
                this._subView = subView;
                this._isSubViewLoaded = false;

                FrameworkElement element;

                if (subView == SubViewType.Disconnected)
                {
                    element = new DisconnectedView();
                }
                else
                {
                    element = new ConnectedView();
                }

                element.Loaded += this.SubView_Loaded;
                this.ContentControl.Content = element;
            }

            //if (e.Error != null)
            //{
            //    this.DisconnectedView.ErrorMessageDisplay(e.Error);
            //}
            //else
            //{
            //    this.DisconnectedView.ErrorMessageClear();
            //}

            if (this._isSubViewLoaded)
            {
                await this.BusyIndicatorUpdate();
            }
        }

        private async Task BusyIndicatorUpdate()
        {
            //this.BusyIndicator.BackgroundImageSource = await this.ContentPanel.BlurEffectApply();
            await Task.CompletedTask;
        }

        private async void ViewModel_StateChanged(object sender, HueSetupViewModelStateChangeEventArgs e)
        {
            switch(this._viewModel.State)
            {
                case HueSetupViewModelState.Disconnected:
                    this.BusyIndicator.Hide();
                    await this.SubViewSet(SubViewType.Disconnected, e.Error);
                    break;

                case HueSetupViewModelState.Connecting:
                    this.BusyIndicator.Text = ResourceManager.HueSetupView_ConnectingText;
                    this.BusyIndicator.Show();
                    break;

                case HueSetupViewModelState.Connected:
                    this.BusyIndicator.Hide();
                    await this.SubViewSet(SubViewType.Connected, e.Error);
                    break;

                case HueSetupViewModelState.Disconnecting:
                    this.BusyIndicator.Text = ResourceManager.HueSetupView_DisconnectingText;
                    this.BusyIndicator.Show();
                    break;
            }
        }

        private async void SubView_Loaded(object sender, RoutedEventArgs e)
        {
            this._isSubViewLoaded = true;

            await this.BusyIndicatorUpdate();
        }
    }
}
