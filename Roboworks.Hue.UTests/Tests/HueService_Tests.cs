using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Roboworks.Hue.UTests.Tests
{
    [TestClass]
    public class HueService_Tests
    {

#region Private Methods

        private HueServiceTestable HueServiceCreate()
        {
            return new HueServiceTestable();
        }

#endregion

        [TestMethod]
        public async Task Light_bulbs_getting_returns_light_bulbs_parsed_from_JSON()
        {
            // Arrange
            var hueService = this.HueServiceCreate();
            hueService.HttpClientGet_ReturnValue = TestData.HueService_LightBulbsGet_Json;

            // Act
            var lightBulbs = await hueService.LightBulbsGet();

            // Assert
            Assert.IsTrue(lightBulbs.Length == TestData.HueService_LightBulbsGet_ItemCount);
        }

        [TestMethod]
        public async Task Light_bulb_ON_setting_returns_TRUE_when_HTTP_response_contains_success()
        {
            // Arrange
            var hueService = this.HueServiceCreate();
            hueService.HttpClientPut_ReturnValue = TestData.HueService_LightBulbIsOnSet_Response;

            // Act
            var isSuccess = await hueService.LightBulbIsOnSet("1", true);

            // Arrange
            Assert.IsTrue(isSuccess);
        }

#region Classes

        public class HueServiceTestable : HueService
        {
            public string HttpClientGet_ReturnValue { get; set; } = null;

            public string HttpClientPut_ReturnValue { get; set; } = null;

            protected override Task<string> HttpClientGet(string requestUri)
            {
                return Task.FromResult(this.HttpClientGet_ReturnValue);
            }

            protected override Task<string> HttpClientPut(string requestUri, string content)
            {
                return Task.FromResult(this.HttpClientPut_ReturnValue);
            }
        }

#endregion

    }
}
