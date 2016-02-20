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

using System.Net;
using System.Text.RegularExpressions;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Roboworks.HueManager.Controls
{
    public sealed partial class IpAddressField : UserControl
    {
        private const string IpAddress_RegexPattern = 
            @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
        private const char IpAddress_PartsDelimiter = '.';
        private const string IpAddress_DefaultValue = "0.0.0.0";

        public static readonly DependencyProperty IpAddressProperty = 
            DependencyProperty.Register(
                "IpAddress", 
                typeof(string), 
                typeof(IpAddressField), 
                new PropertyMetadata(
                    IpAddressField.IpAddress_DefaultValue, 
                    new PropertyChangedCallback(IpAddressField.IpAddress_Changed)
                )
            );

        public string IpAddress
        {
            get
            {
                return (string)this.GetValue(IpAddressField.IpAddressProperty);
            }
            set
            {
                this.SetValue(IpAddressField.IpAddressProperty, value);
            }
        }
        
        public IpAddressField()
        {
            this.InitializeComponent();

            this.IpAddressSet();
        }

#region Private Methods

        private void IpAddressSet()
        {
            this.IpAddress =
                string.Format(
                    "{0}.{1}.{2}.{3}",
                    this.IpAddressPart1.Text,
                    this.IpAddressPart2.Text,
                    this.IpAddressPart3.Text,
                    this.IpAddressPart4.Text
                );
        }

        private void IpAddressParse(string value)
        {
            string newValue;
            
            if (value != null && Regex.IsMatch(value, IpAddressField.IpAddress_RegexPattern))
            {
                newValue = value;
            }
            else
            {
                newValue = IpAddressField.IpAddress_DefaultValue;
            }
            
            var parts = value.Split(IpAddressField.IpAddress_PartsDelimiter);

            this.IpAddressPart1.Text = parts[0];
            this.IpAddressPart2.Text = parts[1];
            this.IpAddressPart3.Text = parts[2];
            this.IpAddressPart4.Text = parts[3];
        }

        private void IpAddressPart_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            var newValue = 
                new String(
                    sender.Text
                        .Where(ch => char.IsDigit(ch))
                        .Select(ch => ch)
                        .ToArray()
                );

            if (sender.Text != newValue)
            {
                var caretIndex = sender.SelectionStart;

                sender.Text = newValue;

                sender.SelectionLength = 0;
                sender.SelectionStart = (caretIndex <= sender.Text.Length) ? caretIndex : sender.Text.Length;
            }
        }

        private void IpAddressPart_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.IpAddressSet();
        }

        private void IpAddressPart_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.SelectionStart = 0;
            textBox.SelectionLength = textBox.Text.Length;
        }

        private void IpAddressPart_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (textBox.Text.Length == 0)
            {
                textBox.Text = default(int).ToString();
            }
        }

        private static void IpAddress_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ipAddressField = (IpAddressField)d;
            ipAddressField.IpAddressParse((string)e.NewValue);
        }

#endregion

    }
}
