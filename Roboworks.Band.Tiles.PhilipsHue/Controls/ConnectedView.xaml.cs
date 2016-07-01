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

namespace Roboworks.Band.Tiles.PhilipsHue.Controls
{
    public sealed partial class ConnectedView : UserControl
    {

#region Properties

        public static readonly DependencyProperty TileAddCommandProperty;
        public ICommand TileAddCommand
        {
            get
            {
                return (ICommand)this.GetValue(ConnectedView.TileAddCommandProperty);
            }
            set
            {
                this.SetValue(ConnectedView.TileAddCommandProperty, value);
            }
        }

        public static readonly DependencyProperty TileRemoveCommandProperty;
        public ICommand TileRemoveCommand
        {
            get
            {
                return (ICommand)this.GetValue(ConnectedView.TileRemoveCommandProperty);
            }
            set
            {
                this.SetValue(ConnectedView.TileRemoveCommandProperty, value);
            }
        }

#endregion

        static ConnectedView()
        {
            ConnectedView.TileAddCommandProperty = 
                DependencyProperty.Register(
                    nameof(ConnectedView.TileAddCommand), 
                    typeof(ICommand), 
                    typeof(ConnectedView),
                    new PropertyMetadata(null)
                );

            ConnectedView.TileRemoveCommandProperty =
                DependencyProperty.Register(
                    nameof(ConnectedView.TileRemoveCommand), 
                    typeof(ICommand), 
                    typeof(ConnectedView),
                    new PropertyMetadata(null)
                );
        }

        public ConnectedView()
        {
            this.InitializeComponent();
        }
    }
}
