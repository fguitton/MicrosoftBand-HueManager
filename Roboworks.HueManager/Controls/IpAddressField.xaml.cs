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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Roboworks.HueManager.Controls
{
    public sealed partial class IpAddressField : UserControl
    {

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
        }

        private void IpAddressPart_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            if (string.IsNullOrEmpty(sender.Text))
            {
                sender.Text = default(int).ToString();
            }
            else
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
                    sender.Text = newValue;
                }
            }
        }

        private void IpAddressPart_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.IpAddress =
                string.Format(
                    "{0}.{1}.{2}.{3}",
                    this.IpAddressPart1,
                    this.IpAddressPart2,
                    this.IpAddressPart3,
                    this.IpAddressPart4
                );
        }

        private static void IpAddress_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ipAddressField = (IpAddressField)d;
            
            // TODO: validate string and set text to text boxes
            // Set default IP address value if validation fails.
        }
    }
}
