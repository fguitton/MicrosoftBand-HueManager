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
    public class HueApiUser_Tests
    {
        [TestMethod]
        public void Parsing_sets_user_ID_to_value_read_from_data()
        {
            // Arrange
            var json = TestData.HueUserCreate_Response;

            // Act
            var hueUser = HueApiUser.FromData(json);

            // Assert
            Assert.AreEqual(TestData.HueBridge_UserId, hueUser.UserId);
        }
    }
}
