using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace Roboworks.Hue
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

        public static HueLightBulb FromData(string id, JToken data)
        {
            return 
                new HueLightBulb()
                {
                    Id = id,
                    Name = (string)data["name"],
                    IsOn = (bool)data["state"]["on"],
                    Brightness = ((double)data["state"]["bri"] - 1) / 253,
                    IsReachable = (bool)data["state"]["reachable"]
                };
        }
    }
}
