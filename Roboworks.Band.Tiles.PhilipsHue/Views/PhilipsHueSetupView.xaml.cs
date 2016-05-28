using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ComponentModel;

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
        private PhilipsHueSetupViewModel _viewModel = null;

        public PhilipsHueSetupView()
        {
            this.InitializeComponent();
        }

#region Private Methods

        private void Page_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (this._viewModel != null)
            {
                this._viewModel.PropertyChanged -= this.ViewModel_PropertyChanged;
            }

            this._viewModel = args.NewValue as PhilipsHueSetupViewModel;
            
            if (this._viewModel != null)
            {
                this._viewModel.PropertyChanged += this.ViewModel_PropertyChanged;

                this.VisuleStateConnectedUpdate(false);
                this.VisuleStateBusyUpdate(false);
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PhilipsHueSetupViewModel.HueBridgeInfo))
            {
                this.VisuleStateConnectedUpdate(true);
            }
            else if (e.PropertyName == nameof(PhilipsHueSetupViewModel.IsBusy))
            {
                this.VisuleStateBusyUpdate(true);
            }
        }

        private void VisuleStateConnectedUpdate(bool useTransition)
        {
            var stateName = 
                this._viewModel.HueBridgeInfo != null ? 
                    this.ConnectedState.Name : 
                    this.DisconnectedState.Name;

            VisualStateManager.GoToState(this, stateName, useTransition);
        }

        private void VisuleStateBusyUpdate(bool useTransition)
        {
            var stateName = this._viewModel.IsBusy ? this.BusyState.Name : this.NotBusyState.Name;

            VisualStateManager.GoToState(this, stateName, useTransition);
        }

#endregion

    }
}
