using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Roboworks.HueManager.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public bool IsInverted { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            object result;

            if (value is bool)
            {
                var boolValue = (bool)value;
                if (this.IsInverted)
                {
                    boolValue = !boolValue;
                }

                result = boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                result = value;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
