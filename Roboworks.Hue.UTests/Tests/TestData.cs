using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboworks.Hue.UTests.Tests
{
    public static class TestData
    {

#region HueLightBulb Data

        public const string HueLightBulb_Id = "1";
        public const string HueLightBulb_Name = "Living room";
        public const bool HueLightBulb_IsOn = true;
        public const byte HueLightBulb_Brightness = 254;
        public const bool HueLightBulb_IsReachable = false;

        public const string HueLightBulb_Json =
            @"{
                ""state"": {
                    ""on"": true,
                    ""bri"": 254,
                    ""alert"": ""none"",
                    ""reachable"": false
                },
                ""type"": ""Dimmable light"",
                ""name"": ""Living room"",
                ""modelid"": ""LWB004"",
                ""manufacturername"": ""Philips"",
                ""uniqueid"": ""00:17:88:01:00:e4:c5:f6-0b"",
                ""swversion"": ""66012040""
            }";

#endregion

#region HueService Data

        public const int HueService_LightBulbsGet_ItemCount = 2;

        public const string HueService_LightBulbIsOnSet_Response =
            @"[
                {
                    ""success"":
                    {
                        ""/lights/1/state/on"":false
                    }
                }
            ]";

        public const string HueService_LightBulbsGet_Json =
            @"{
                ""1"": {
                    ""state"": {
                        ""on"": true,
                        ""bri"": 254,
                        ""alert"": ""none"",
                        ""reachable"": false
                    },
                    ""type"": ""Dimmable light"",
                    ""name"": ""Living room"",
                    ""modelid"": ""LWB004"",
                    ""manufacturername"": ""Philips"",
                    ""uniqueid"": ""00:17:88:01:00:e4:c5:f6-0b"",
                    ""swversion"": ""66012040""
                },
                ""2"": {
                    ""state"": {
                        ""on"": true,
                        ""bri"": 254,
                        ""alert"": ""none"",
                        ""reachable"": false
                    },
                    ""type"": ""Dimmable light"",
                    ""name"": ""Bedroom"",
                    ""modelid"": ""LWB004"",
                    ""manufacturername"": ""Philips"",
                    ""uniqueid"": ""00:17:88:01:00:e4:ab:ab-0b"",
                    ""swversion"": ""66012040""
                }
            }";

#endregion

    }
}
