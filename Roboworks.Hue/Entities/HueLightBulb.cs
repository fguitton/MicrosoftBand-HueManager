using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace Roboworks.Hue.Entities
{
    public class HueLightBulb
    {
        public string Id { get; private set; }

        public string Name { get; private set; }

        public bool IsOn { get;  private set; }

        public double Brightness { get; private set; }

        public bool IsReachable { get; private set; }

        private HueLightBulb()
        {
        }

        internal static HueLightBulb FromData(string id, string data)
        {
            var jToken = JToken.Parse(data);

            return 
                new HueLightBulb()
                {
                    Id = id,
                    Name = (string)jToken["name"],
                    IsOn = (bool)jToken["state"]["on"],
                    Brightness = ((double)jToken["state"]["bri"] - 1) / 253,
                    IsReachable = (bool)jToken["state"]["reachable"]
                };
        }
    }
}
