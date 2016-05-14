using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Data;

using Roboworks.Band.Common.Controls;
using Roboworks.Hue;

namespace Roboworks.Band.Tiles.PhilipsHue.Converters
{
    public class ExceptionToErrorInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ErrorInfo errorInfo = null;
            var exception = value as Exception;

            if (exception != null)
            {
                string message;

                if (exception is HueApiResponseErrorException)
                {
                    var hueException = (HueApiResponseErrorException)exception;
                    message = ResourceManager.ErrorMessageGet(hueException.ErrorType);
                }
                else
                {
                    message = ResourceManager.HueSetupView_ConnectFailedMessage;
                }

                errorInfo = new ErrorInfo(ResourceManager.HueSetupView_ConnectFailedTitle, message);
            }

            return errorInfo;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
