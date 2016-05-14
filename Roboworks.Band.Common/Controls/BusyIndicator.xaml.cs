using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Streams;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;

namespace Roboworks.Band.Common.Controls
{
    public sealed partial class BusyIndicator : UserControl
    {

#region Properties

        public static readonly DependencyProperty TextProperty;
        public string Text
        {
            get
            {
                return (string)this.GetValue(BusyIndicator.TextProperty);
            }
            set
            {
                this.SetValue(BusyIndicator.TextProperty, value);
            }
        }

#endregion

        static BusyIndicator()
        {
            BusyIndicator.TextProperty = 
                DependencyProperty.Register(
                    nameof(BusyIndicator.Text), 
                    typeof(string), 
                    typeof(BusyIndicator),
                    new PropertyMetadata(null)
                );
        }

        public BusyIndicator()
        {
            this.InitializeComponent();
        }

    }
}
