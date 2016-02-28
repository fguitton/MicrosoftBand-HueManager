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
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Roboworks.Band.Common.Controls
{
    public sealed partial class ErrorBox : UserControl
    {
        public static readonly DependencyProperty TitleProperty = 
            DependencyProperty.Register(
                "Title",
                typeof(string),
                typeof(ErrorBox),
                new PropertyMetadata(null)
            );

        public static readonly DependencyProperty MessageProperty = 
            DependencyProperty.Register(
                "Message",
                typeof(string),
                typeof(ErrorBox),
                new PropertyMetadata(null)
            );

#region Properties

        public string Title
        {
            get
            {
                return (string)this.GetValue(ErrorBox.TitleProperty);
            }
            set
            {
                this.SetValue(ErrorBox.TitleProperty, value);
            }
        }

        public string Message
        {
            get
            {
                return (string)this.GetValue(ErrorBox.MessageProperty);
            }
            set
            {
                this.SetValue(ErrorBox.MessageProperty, value);
            }
        }

#endregion

        public ErrorBox()
        {
            this.InitializeComponent();
        }

#region Private Methods
        
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            MessageService.DialogShow(this.Title, this.Message);
        }

#endregion

    }
}
