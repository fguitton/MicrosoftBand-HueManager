using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboworks.Hue.UTests
{
    public static class TestData
    {

#region Errors

        public const string HueApiResponse_LinkButtonNotPressedError =
            @"[
                {
                    ""error"":
                    {
                        ""type"":101,
                        ""address"":"""",
                        ""description"":""link button not pressed""
                    }
                }
            ]";

        public const string HueApiResponse_ResourceIsNotAvailableError =
            @"[
                {
                    ""error"": 
                    {
                        ""type"": 3,
                        ""address"": ""/config/whitelist/3f9b67b712a30a1fb74ada73242c22f"",
                        ""description"": ""resource, /config/whitelist/3f9b67b712a30a1fb74ada73242c22f, not available""
                    }
                }
            ]";

#endregion

#region HueService - Ping

        public const string HueBridgePing_RequestUri = @"http://192.168.1.100";

        public const string HueBridgePing_Response = 
            @"
                <!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">
                <html>
                    <head>
                        <title>hue personal wireless lighting</title>
                    </head>
                </html>
            ";

#endregion

#region HueService - HueBridge Info

        public const string HueBridge_IpAddress = "192.168.1.100";
        public const string HueBridge_AppName = "UTest";
        public const string HueBridge_UserId = "188aea1620420fc73c820a7a3b07cdb";

#endregion

#region HueService - HueBridgeConfig API

        public const string HueBridgeConfig_Name = "Philips hue";

        public const string HueBridgeConfig_RequestUrl = 
            "http://192.168.1.100/api/188aea1620420fc73c820a7a3b07cdb/config";

        public const string HueBridgeConfig_ResponseData = 
            @"{
                ""name"": ""Philips hue"",
                ""zigbeechannel"": 15,
                ""bridgeid"": ""001788FFFE197C57"",
                ""mac"": ""00:17:88:19:7c:57"",
                ""dhcp"": true,
                ""ipaddress"": ""192.168.1.100"",
                ""netmask"": ""255.255.255.0"",
                ""gateway"": ""192.168.1.1"",
                ""proxyaddress"": ""none"",
                ""proxyport"": 0,
                ""UTC"": ""2016-01-24T21:18:45"",
                ""localtime"": ""2016-01-24T21:18:45"",
                ""timezone"": ""Europe/London"",
                ""modelid"": ""BSB001"",
                ""swversion"": ""01030262"",
                ""apiversion"": ""1.11.0"",
                ""swupdate"": {
                    ""updatestate"": 0,
                    ""checkforupdate"": false,
                    ""devicetypes"": {
                        ""bridge"": false,
                        ""lights"": [],
                        ""sensors"": []
                    },
                    ""url"": """",
                    ""text"": """",
                    ""notify"": false
                },
                ""linkbutton"": false,
                ""portalservices"": true,
                ""portalconnection"": ""connected"",
                ""portalstate"": {
                    ""signedon"": true,
                    ""incoming"": true,
                    ""outgoing"": true,
                    ""communication"": ""disconnected""
                },
                ""factorynew"": false,
                ""replacesbridgeid"": null,
                ""backup"": {
                    ""status"": ""idle"",
                    ""errorcode"": 0
                },
                ""whitelist"": {
                    ""phTlhVxdy82ddIPo"": {
                        ""last use date"": ""2015-12-19T10:14:28"",
                        ""create date"": ""2015-09-28T18:20:56"",
                        ""name"": ""Hue#iPad""
                    },
                    ""huetrohuetro"": {
                        ""last use date"": ""2016-01-09T16:59:10"",
                        ""create date"": ""2015-09-28T18:39:22"",
                        ""name"": ""Huetro (Windows Phone)""
                    },
                    ""jpCoMbV5PJNXyGLW"": {
                        ""last use date"": ""2016-01-01T03:42:06"",
                        ""create date"": ""2015-09-28T20:52:15"",
                        ""name"": ""Hue#iPhone Kristina Cam""
                    },
                    ""188aea1620420fc73c820a7a3b07cdb"": {
                        ""last use date"": ""2016-01-24T21:18:45"",
                        ""create date"": ""2015-09-29T09:02:13"",
                        ""name"": ""my_hue_app#test""
                    },
                    ""pcW4ZFW25qOsjYHe"": {
                        ""last use date"": ""2016-01-23T18:13:25"",
                        ""create date"": ""2015-12-23T22:11:40"",
                        ""name"": ""Hue#Pavels's iPad""
                    }
                }
            }";

#endregion

#region HueService - HueLightBulb API

        public const string HueLightBulb_Id = "1";
        public const string HueLightBulb_Name = "Living room";
        public const bool HueLightBulb_IsOn = true;
        public const double HueLightBulb_Brightness = 1.0;
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

#region HueService - HueUser API
        
        public const string HueUserCreate_Response = 
            @"[{""success"":{""username"": ""188aea1620420fc73c820a7a3b07cdb""}}]";

        public const string HueUserDelete_Response = 
            @"[{""success"": ""/config/whitelist/188aea1620420fc73c820a7a3b07cdb deleted""}]";

        public const string HueUser_ResourceLocation = "/config/whitelist/188aea1620420fc73c820a7a3b07cdb";
        
#endregion

#region HueService - Data

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
