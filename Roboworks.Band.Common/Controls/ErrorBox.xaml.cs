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

#region Properties

        public static readonly DependencyProperty ErrorInfoProperty;
        public ErrorInfo ErrorInfo
        {
            get
            {
                return (ErrorInfo)this.GetValue(ErrorBox.ErrorInfoProperty);
            }
            set
            {
                this.SetValue(ErrorBox.ErrorInfoProperty, value);
            }
        }

#endregion

        static ErrorBox()
        {
            ErrorBox.ErrorInfoProperty = 
                DependencyProperty.Register(
                    nameof(ErrorBox.ErrorInfo), 
                    typeof(ErrorInfo), 
                    typeof(ErrorBox),
                    new PropertyMetadata(null)
                );
        }

        public ErrorBox()
        {
            this.InitializeComponent();
        }

#region Private Methods
        
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ErrorInfo != null)
            {
                MessageService.DialogShow(this.ErrorInfo.Title, this.ErrorInfo.Message);
            }
        }

#endregion



    }

    public class ErrorInfo
    {
        public string Title { get; }

        public string Message { get; }

        public ErrorInfo(string title, string message)
        {
            if (title == null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            this.Title = title;
            this.Message = message;
        }
    }
}
