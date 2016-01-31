using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Roboworks.Hue
{
    public class HueApiResponseErrorException : HueException
    {
        public HueErrorType ErrorType { get; }

        public HueApiResponseErrorException(HueErrorType errorType)
            : this(errorType, null)
        {
        }

        public HueApiResponseErrorException(HueErrorType errorType, string message) 
            : this(errorType, message, null)
        {
        }

        public HueApiResponseErrorException(HueErrorType errorType, string message, Exception inner) 
            : base(message, inner)
        {
            this.ErrorType = errorType;
        }
    }
}
