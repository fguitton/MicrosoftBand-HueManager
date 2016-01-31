using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace Roboworks.Hue
{
    internal static class HueHelper
    {
        public static string HueApiUriGet(string ipAddress)
        {
            if (ipAddress == null)
            {
                throw new ArgumentNullException(nameof(ipAddress));
            }

            return $"http://{ipAddress}/api";
        }

        public static string HueApiWithUserUriGet(string ipAddress, string hueApiUserId)
        {
            if (ipAddress == null)
            {
                throw new ArgumentNullException(nameof(ipAddress));
            }

            if (hueApiUserId == null)
            {
                throw new ArgumentNullException(nameof(hueApiUserId));
            }
            
            return $"http://{ipAddress}/api/{hueApiUserId}";
        }

        public static void HueApiResponseErrorCheck(string data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var error = 
                JArray.Parse(data)
                    .Cast<JObject>()
                    .SelectMany(jObject => jObject.Properties())
                    .FirstOrDefault(item => item.Name == "error");

            if (error != null)
            {
                throw 
                    new HueApiResponseErrorException(
                        (HueErrorType)(int)error.Value["type"], 
                        (string)error.Value["description"]
                    );
            }
        }

        public static void HueApiResponseSuccessDeleteCheck(string data, string resourceLocation)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var successProperty = 
                JArray.Parse(data)
                    .Cast<JObject>()
                    .SelectMany(jObject => jObject.Properties())
                    .FirstOrDefault(item => item.Name == "success");

            if ((string)successProperty?.Value != $"{resourceLocation} deleted")
            {
                throw new HueException($"\"{resourceLocation}\" has not been deleted.");
            }
        }
    }
}
