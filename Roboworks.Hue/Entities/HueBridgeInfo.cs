using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace Roboworks.Hue.Entities
{
    public class HueBridgeInfo
    {
        public string Id { get; private set; }

        public string Name { get; private set; }
        
        public string IpAddress { get; private set; }

        private HueBridgeInfo()
        {
        }

        internal static HueBridgeInfo FromData(string data)
        {
            var jObject = JObject.Parse(data);

            return 
                new HueBridgeInfo()
                {
                    Id = (string)jObject["bridgeid"],
                    Name = (string)jObject["name"],
                    IpAddress = (string)jObject["ipaddress"]
                };
        }
    }
}
