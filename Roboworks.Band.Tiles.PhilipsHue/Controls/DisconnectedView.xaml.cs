using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;

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
using Roboworks.Band.Common.Controls;

namespace Roboworks.Band.Tiles.PhilipsHue.Controls
{
    public sealed partial class DisconnectedView : UserControl
    {

#region Properties

        public static readonly DependencyProperty IpAddressProperty;
        public string IpAddress
        {
            get
            {
                return (string)this.GetValue(DisconnectedView.IpAddressProperty);
            }
            set
            {
                this.SetValue(DisconnectedView.IpAddressProperty, value);
            }
        }

        public static readonly DependencyProperty ConnectCommandProperty;
        public ICommand ConnectCommand
        {
            get
            {
                return (ICommand)this.GetValue(DisconnectedView.ConnectCommandProperty);
            }
            set
            {
                this.SetValue(DisconnectedView.ConnectCommandProperty, value);
            }
        }

        public static readonly DependencyProperty ErrorInfoProperty;
        public ErrorInfo ErrorInfo
        {
            get
            {
                return (ErrorInfo)this.GetValue(DisconnectedView.ErrorInfoProperty);
            }
            set
            {
                this.SetValue(DisconnectedView.ErrorInfoProperty, value);
            }
        }
        
#endregion

        static DisconnectedView()
        {
            DisconnectedView.IpAddressProperty = 
                DependencyProperty.Register(
                    nameof(DisconnectedView.IpAddress), 
                    typeof(string), 
                    typeof(DisconnectedView),
                    new PropertyMetadata(null)
                );

            DisconnectedView.ConnectCommandProperty = 
                DependencyProperty.Register(
                    nameof(DisconnectedView.ConnectCommand), 
                    typeof(ICommand), 
                    typeof(DisconnectedView),
                    new PropertyMetadata(null)
                );

            DisconnectedView.ErrorInfoProperty = 
                DependencyProperty.Register(
                    nameof(DisconnectedView.ErrorInfo), 
                    typeof(ErrorInfo), 
                    typeof(DisconnectedView),
                    new PropertyMetadata(null)
                );
        }

        public DisconnectedView()
        {
            this.InitializeComponent();
        }
        
    }
}
