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

        //[TestInitialize]
        //public void Initialize()
        //{
        //    this._hueSettingsProviderMock = new HueSettingsProviderMock();
        //}

#region Private Methods

        //private HueServiceTestable HueServiceCreate()
        //{
        //    return null; //new HueServiceTestable(this._hueSettingsProviderMock);
        //}

#endregion

        //[TestMethod]
        //public async Task Initializing_returns_FALSE_when_Hue_bridge_IP_address_is_unknown()
        //{
        //    // Arrange
        //    var hueService = this.HueServiceCreate();

        //    this._hueSettingsProviderMock.HueApiIpAddress = null;

        //    // Act
        //    var value = await hueService.Initialize();

        //    // Assert
        //    Assert.IsFalse(value);
        //}

        //[TestMethod]
        //public async Task Initializing_returns_FALSE_when_Hue_API_user_ID_is_unknown()
        //{
        //    // Arrange
        //    var hueService = this.HueServiceCreate();

        //    this._hueSettingsProviderMock.HueApiUserId = null;

        //    // Act
        //    var value = await hueService.Initialize();

        //    // Assert
        //    Assert.IsFalse(value);
        //}

        //[TestMethod]
        //public async Task Initializing_returns_TRUE_when_Hue_API_user_ID_and_Hue_bridge_IP_address_are_known()
        //{
        //    // Arrange
        //    var hueService = this.HueServiceCreate();

        //    // Act
        //    var value = await hueService.Initialize();

        //    // Assert
        //    Assert.IsTrue(value);
        //}
        
        //[TestMethod]
        //public async Task Initializing_sets_Hue_bridge_information_with_data_from_GET_request_to_hue_bridge_config_URL()
        //{
        //    // Arrange
        //    var hueService = this.HueServiceCreate();

        //    hueService.HttpClientGet_Mock =
        //        (requestUri) => 
        //            Value
        //                .When(requestUri)
        //                .Equals(TestData.HueBridgeConfig_RequestUrl)
        //                .ReturnAsync(TestData.HueBridgeConfig_ResponseData);

        //    // Act
        //    await hueService.Initialize();

        //    // Assert
        //    Assert.IsNotNull(TestData.HueBridgeConfig_Name, hueService.HueBridgeInfo.Name);
        //}

        //[TestMethod]
        //public async Task Light_bulbs_getting_returns_light_bulbs_parsed_from_JSON()
        //{
        //    // Arrange
        //    var hueService = this.HueServiceCreate();
        //    hueService.HttpClientGet_Mock = 
        //        (requestUri) => Task.FromResult(TestData.HueService_LightBulbsGet_Json);

        //    // Act
        //    var lightBulbs = await hueService.LightBulbsGet();

        //    // Assert
        //    Assert.IsTrue(lightBulbs.Length == TestData.HueService_LightBulbsGet_ItemCount);
        //}

        //[TestMethod]
        //public async Task Light_bulb_ON_setting_returns_TRUE_when_HTTP_response_contains_success()
        //{
        //    // Arrange
        //    var hueService = this.HueServiceCreate();
        //    hueService.HttpClientPut_Mock = 
        //        (requestUri, content) => Task.FromResult(TestData.HueService_LightBulbIsOnSet_Response);

        //    // Act
        //    var isSuccess = await hueService.LightBulbIsOnSet("1", true);

        //    // Arrange
        //    Assert.IsTrue(isSuccess);
        //}

#region Classes

        //public class HueServiceTestable : HueService
        //{
        //    public delegate Task<string> HttpClientGetDelegate(string requestUri);

        //    public delegate Task<string> HttpClientPutDelegate(string requestUri, string content);

        //    public HttpClientGetDelegate HttpClientGet_Mock { get; set; } =
        //        (requestUri) => Task.FromResult(default(string));

        //    public HttpClientPutDelegate HttpClientPut_Mock { get; set; } =
        //        (requestUri, content) => Task.FromResult(default(string));

        //    public HueServiceTestable(IHueSettingsProvider hueSettingsProvider)
        //        : base(hueSettingsProvider)
        //    {
        //    }

        //    protected override Task<string> HttpClientGet(string requestUri)
        //    {
        //        return this.HttpClientGet_Mock.Invoke(requestUri);
        //    }

        //    protected override Task<string> HttpClientPut(string requestUri, string content)
        //    {
        //        return this.HttpClientPut_Mock.Invoke(requestUri, content);
        //    }
        //}

#endregion

    }
}
