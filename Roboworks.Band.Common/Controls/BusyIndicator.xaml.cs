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

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(BusyIndicator),
                new PropertyMetadata(null)
            );

        public static readonly DependencyProperty BackgroundImageSourceProperty =
            DependencyProperty.Register(
                "BackgroundImageSource",
                typeof(ImageSource),
                typeof(BusyIndicator),
                new PropertyMetadata(null)
            );

#region Properties

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

        public ImageSource BackgroundImageSource
        {
            get
            {
                return (ImageSource)this.GetValue(BusyIndicator.BackgroundImageSourceProperty);
            }
            set
            {
                this.SetValue(BusyIndicator.BackgroundImageSourceProperty, value);
            }
        }

#endregion

        public BusyIndicator()
        {
            this.InitializeComponent();
        }

#region Public Methods

        public void Show()
        {
            this.RootPanel.Visibility = Visibility.Visible;
            this.ProgressRing.IsActive = true;
        }

        public void Hide()
        {
            this.RootPanel.Visibility = Visibility.Collapsed;
            this.ProgressRing.IsActive = false;
        }

#endregion

    }
}
