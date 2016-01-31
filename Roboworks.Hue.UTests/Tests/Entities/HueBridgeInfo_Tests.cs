using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

using Roboworks.Hue.Entities;

namespace Roboworks.Hue.UTests.Tests.Entities
{
    [TestClass]
    public class HueBridgeInfo_Tests
    {
        [TestMethod]
        public void Parsing_sets_name_to_value_read_from_data()
        {
            // Arrange
            var data = TestData.HueBridgeConfig_ResponseData;

            // Act
            var hueBridgeInfo = HueBridgeInfo.FromData(data);

            // Assert
            Assert.AreEqual(TestData.HueBridgeConfig_Name, hueBridgeInfo.Name);
        }
    }
}
