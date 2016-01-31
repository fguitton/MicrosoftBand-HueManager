using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

using Roboworks.Hue.UTests.Mock;

namespace Roboworks.Hue.UTests.Tests
{
    [TestClass]
    public class HueServiceProvider_Tests
    {
        private HttpClientMock _httpClientMock;

        [TestInitialize]
        public void Initialize()
        {
            this._httpClientMock = new HttpClientMock();
        }

        private HueServiceProvider HueServiceProviderCreate()
        {
            return new HueServiceProvider(this._httpClientMock);
        }

        [TestMethod]
        public async Task Hue_API_user_creation_throws_Hue_exception_when_link_button_is_not_pressed()
        {
            // Arrange
            var hueServicePrivider = this.HueServiceProviderCreate();

            this._httpClientMock.HttpClientPost_Delegate = 
                (requestUri, content) => Task.FromResult(TestData.HueApiResponse_LinkButtonNotPressedError);

            HueApiResponseErrorException exception = null;

            // Act
            try
            {
                await hueServicePrivider.HueApiUserCreate(
                    TestData.HueBridge_IpAddress, 
                    TestData.HueBridge_AppName
                );
            }
            catch(HueApiResponseErrorException ex)
            {
                exception = ex;
            }

            // Assert
            Assert.AreEqual(HueErrorType.LinkButtonNotPressed, exception.ErrorType);
        }

        [TestMethod]
        public async Task Hue_API_user_creation_returns_Hue_user_with_ID_read_from_API_response()
        {
            // Arrange
            var hueServicePrivider = this.HueServiceProviderCreate();

            this._httpClientMock.HttpClientPost_Delegate = 
                (requestUri, content) => Task.FromResult(TestData.HueUserCreate_Response);
            
            // Act
            var hueUser = await 
                hueServicePrivider.HueApiUserCreate(
                    TestData.HueBridge_IpAddress, 
                    TestData.HueBridge_AppName
                );

            // Assert
            Assert.AreEqual(TestData.HueBridge_UserId, hueUser.UserId);
        }

        [TestMethod]
        public async Task Hue_API_user_deleting_does_not_throw_exceptin_when_Hue_API_successfuly_deletes_resource()
        {
            // Arrange
            var hueServicePrivider = this.HueServiceProviderCreate();

            this._httpClientMock.HttpClientDelete_Delegate = 
                (requestUri) => Task.FromResult(TestData.HueUserDelete_Response);

            Exception exception = null;
            
            // Act
            try
            {
                await 
                    hueServicePrivider.HueApiUserDelete(
                        TestData.HueBridge_IpAddress, 
                        TestData.HueBridge_UserId
                    );
            }
            catch(Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.IsNull(exception);
        }
    }
}
