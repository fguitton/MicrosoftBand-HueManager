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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Roboworks.Band.Common.Controls
{
    public sealed partial class PageHeader : UserControl
    {
        public static DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title", 
                typeof(string), 
                typeof(PageHeader), 
                new PropertyMetadata(null)
            );

        public string Title
        {
            get
            {
                return (string)this.GetValue(PageHeader.TitleProperty);
            }
            set
            {
                this.SetValue(PageHeader.TitleProperty, value);
            }
        }

        public PageHeader()
        {
            this.InitializeComponent();
        }
    }
}
