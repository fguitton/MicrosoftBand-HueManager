using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Roboworks.HueManager.ViewModels;
using Roboworks.Hue;
using Windows.UI.Popups;

namespace Roboworks.HueManager.Views
{
    public sealed partial class HueSetupView : Page
    {
        private static readonly TimeSpan LoadingViewDelayMinimumTime = TimeSpan.FromSeconds(1);

        private Task _loadingViewDelay = null;

        private HueSetupViewModel _viewModel;

        public HueSetupView()
        {
            this.InitializeComponent();
        }

#region Private Methods

        private void LoadingViewDelaySet()
        {
            this._loadingViewDelay = Task.Delay(HueSetupView.LoadingViewDelayMinimumTime);
        }

        private async Task LoadingViewDelayAwaitAsync()
        {
            if (this._loadingViewDelay != null)
            {
                await this._loadingViewDelay;
                this._loadingViewDelay = null;
            }
        }

        private void ErrorMessageDisplay(Exception error)
        {
            string message;

            if (error is HueApiResponseErrorException)
            {
                var hueException = (HueApiResponseErrorException)error;
                message = ResourceManager.ErrorMessageGet(hueException.ErrorType);
            }
            else
            {
                message = ResourceManager.HueSetupView_ConnectFailedMessage;
            }

            MessageService.DialogShow(
                ResourceManager.HueSetupView_ConnectFailedTitle, 
                message
            );
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

        private void VisualStateInit(HueSetupViewModelState state)
        {
            string stateName;

            switch(state)
            {
                case HueSetupViewModelState.Disconnected:
                    stateName = this.DisconnectingState.Name;
                    break;

                case HueSetupViewModelState.Connecting:
                    stateName = this.ConnectingState.Name;
                    break;

                case HueSetupViewModelState.Connected:
                    stateName = this.ConnectedState.Name;
                    break;

                case HueSetupViewModelState.Disconnecting:
                    stateName = this.DisconnectingState.Name;
                    break;

                default:
                    throw new NotSupportedException();
            }

            VisualStateManager.GoToState(this, stateName, true);
        }

        private async void ViewModel_StateChanged(object sender, HueSetupViewModelStateChangeEventArgs e)
        {
            var viewModel = (HueSetupViewModel)sender;
            var useTransitions = !e.IsInitialState;
            
            switch(viewModel.State)
            {
                case HueSetupViewModelState.Disconnected:
                    await this.LoadingViewDelayAwaitAsync();

                    VisualStateManager.GoToState(this, this.DisconnectedState.Name, useTransitions);

                    if (e.Error != null)
                    {
                        EventHandler<object> eventHandler = null;
                        eventHandler =
                            (sender_, args) =>
                            {
                                this.ErrorMessageDisplay(e.Error);
                                this.ToDisconnectedTransition.Completed -= eventHandler;
                            };

                        this.ToDisconnectedTransition.Completed += eventHandler;
                    }
                    break;

                case HueSetupViewModelState.Connecting:
                    if (e.IsInitialState)
                    {
                        VisualStateManager.GoToState(this, this.ConnectingState.Name, false);
                    }

                    this.LoadingViewDelaySet();
                    break;

                case HueSetupViewModelState.Connected:
                    await this.LoadingViewDelayAwaitAsync();

                    VisualStateManager.GoToState(this, this.ConnectedState.Name, useTransitions);

                    if (e.Error != null)
                    {
                        this.ErrorMessageDisplay(e.Error);
                    }
                    break;

                case HueSetupViewModelState.Disconnecting:
                    if (e.IsInitialState)
                    {
                        VisualStateManager.GoToState(this, this.DisconnectingState.Name, false);
                    }

                    this.LoadingViewDelaySet();
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

#endregion
        
    }
}
