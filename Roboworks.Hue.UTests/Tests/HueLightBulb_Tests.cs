using System;

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

using Newtonsoft.Json.Linq;

namespace Roboworks.Hue.UTests.Tests
{
    [TestClass]
    public class HueLightBulb_Tests
    {
        [TestMethod]
        public void Parse_sets_id_to_passed_argument_value()
        {
            // Arrange
            var data = JToken.Parse(TestData.HueLightBulb_Json);

            // Act
            var lightBulb = HueLightBulb.FromData(TestData.HueLightBulb_Id, data);

            // Assert
            Assert.AreEqual(TestData.HueLightBulb_Id, lightBulb.Id);
        }

        [TestMethod]
        public void Parse_sets_name_to_value_extracted_from_JSON()
        {
            // Arrange
            var data = JToken.Parse(TestData.HueLightBulb_Json);

            // Act
            var lightBulb = HueLightBulb.FromData(TestData.HueLightBulb_Id, data);

            // Assert
            Assert.AreEqual(TestData.HueLightBulb_Name, lightBulb.Name);
        }

        [TestMethod]
        public void Parse_sets_on_to_value_extracted_from_JSON()
        {
            // Arrange
            var data = JToken.Parse(TestData.HueLightBulb_Json);

            // Act
            var lightBulb = HueLightBulb.FromData(TestData.HueLightBulb_Id, data);

            // Assert
            Assert.AreEqual(TestData.HueLightBulb_IsOn, lightBulb.IsOn);
        }

        [TestMethod]
        public void Parse_sets_brightness_to_value_extracted_from_JSON()
        {
            // Arrange
            var data = JToken.Parse(TestData.HueLightBulb_Json);

            // Act
            var lightBulb = HueLightBulb.FromData(TestData.HueLightBulb_Id, data);

            // Assert
            Assert.AreEqual(TestData.HueLightBulb_Brightness, lightBulb.Brightness);
        }

        [TestMethod]
        public void Parse_sets_reachable_to_value_extracted_from_JSON()
        {
            // Arrange
            var data = JToken.Parse(TestData.HueLightBulb_Json);

            // Act
            var lightBulb = HueLightBulb.FromData(TestData.HueLightBulb_Id, data);

            // Assert
            Assert.AreEqual(TestData.HueLightBulb_IsReachable, lightBulb.IsReachable);
        }
    }
}
