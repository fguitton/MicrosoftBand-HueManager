using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Roboworks.Hue.ITests.Tests
{
    [TestClass]
    public class HueServiceProvider_Tests
    {
        
        private HueServiceProvider HueServiceProviderCreate()
        {
            return new HueServiceProvider();
        }

        [TestMethod]
        public async Task Hue_user_creation_does_not_return_NULL()
        {
            // Prerequisites
            /* 
            Press link button before running this test.
            */
            
            // Arrange
            var hueServicePrivider = this.HueServiceProviderCreate();

            // Act
            var hueUser =
                await hueServicePrivider.HueApiUserCreate(
                    TestData.HueBridge_IpAddress, 
                    TestData.HueBridge_AppName
                );

            // Assert
            Assert.IsNotNull(hueUser);

            // Cleanup
            await hueServicePrivider.HueApiUserDelete(TestData.HueBridge_IpAddress, hueUser.UserId);
        }

        [TestMethod]
        public async Task Hue_service_getter_returns_Hue_service_with_Hue_bridge_IP_address_eqaul_to_passed_IP_address()
        {
            // Arrange
            var hueServicePrivider = this.HueServiceProviderCreate();
            
            // Act
            var hueService =
                await hueServicePrivider.Connect(TestData.HueBridge_IpAddress, TestData.HueBridge_UserId);

            // Assert
            Assert.IsNotNull(TestData.HueBridge_IpAddress, hueService.HueBridgeInfo.IpAddress);
        }
    }
}
