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

using Roboworks.Band.Tiles.PhilipsHue.ViewModels;
using Roboworks.Hue;
using Roboworks.Band.Common;

namespace Roboworks.Band.Tiles.PhilipsHue.Controls
{
    public sealed partial class DisconnectedView : UserControl
    {
        public DisconnectedView()
        {
            this.InitializeComponent();
        }

        public void ErrorMessageDisplay(Exception error)
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

            this.ErrorBox.Title = ResourceManager.HueSetupView_ConnectFailedTitle;
            this.ErrorBox.Message = message;
        }

        public void ErrorMessageClear()
        {
            this.ErrorBox.Title = null;
            this.ErrorBox.Message = null;
        }

    }
}
