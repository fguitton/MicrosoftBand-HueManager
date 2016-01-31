using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

using Roboworks.Hue.Entities;

namespace Roboworks.Hue.ITests.Tests
{
    [TestClass]
    public class HueService_Tests
    {

#region Private Methods

        private HueService HueServiceCreate()
        {
            return 
                new HueService(
                    HueBridgeInfo.FromData(TestData.HueBridgeInfo_Json),
                    TestData.HueBridge_UserId, 
                    new HttpClient()
                );
        }

#endregion

        [TestMethod]
        public async Task Light_bulbs_getting_returns_array_of_light_bulbs()
        {
            // Arrange
            var hueService = this.HueServiceCreate();

            // Act
            var lightBulbs = await hueService.LightBulbsGet();

            // Assert
            Assert.IsNotNull(lightBulbs);
        }

        [TestMethod]
        public async Task Light_bulb_ON_setting_does_not_throw_exception()
        {
            // Arrange
            var hueService = this.HueServiceCreate();
            Exception exception = null;

            // Act
            try
            {
                await hueService.LightBulbIsOnSet("1", true);
            }
            catch(Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.IsNull(exception);
        }

        [TestMethod]
        public async Task Light_bulb_brightness_setting_does_not_throw_exception()
        {
            // Arrange
            var hueService = this.HueServiceCreate();
            Exception exception = null;

            // Act
            try
            {
                await hueService.LightBulbBrightnessSet("1", 0.1d);
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
