using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboworks.Hue.ITests
{
    public static class TestData
    {
        public const string HueBridge_IpAddress = "192.168.1.100";
        public const string HueBridge_AppName = "ITest";
        public const string HueBridge_UserId = "188aea1620420fc73c820a7a3b07cdb";

        public const string HueBridgeInfo_Json = 
            @"{
                ""name"": ""Philips hue"",
                ""bridgeid"": ""001788FFFE197C57"",
                ""ipaddress"": ""192.168.1.100""
            }";
    }
}
