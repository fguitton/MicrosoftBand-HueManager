using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace Roboworks.Hue.Entities
{
    public class HueApiUser
    {
        public string UserId { get; private set; }

        private HueApiUser()
        {
        }

        internal static HueApiUser FromData(string json)
        {
            var jToken = JToken.Parse(json);
               
            return
                new HueApiUser()
                {
                    UserId = (string)jToken[0]["success"]["username"]
                };
        }
    }
}
