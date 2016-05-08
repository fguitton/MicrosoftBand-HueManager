using System;

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Roboworks.Band.Common.UTests.Tests
{
    [TestClass]
    public class Extensions_Tests
    {

#region Tests

        [DataTestMethod]
        [DataRow(-0.001d)]
        [DataRow(1.001d)]
        public void Pertentage_throws_argument_out_of_range_exception_when_passed_value_is_less_then_0_or_geater_then_1(double value)
        {
             // Arrange
            Exception exception = null;

            // Act
            try
            {
                value.ToPercentage();
            }
            catch(Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.IsInstanceOfType(exception, typeof(ArgumentOutOfRangeException));
        }

        [DataTestMethod]
        [DataRow(0d, 0)]
        [DataRow(0.005d, 1)]
        [DataRow(0.5d, 50)]
        [DataRow(0.995d, 99)]
        [DataRow(1d, 100)]
        public void Pertentage_returns_expected_result(double value, int expectedResult)
        {
            // Arrange
            int percentage;

            // Act
            percentage = value.ToPercentage();

            // Assert
            Assert.AreEqual(expectedResult, percentage);
        }

#endregion

    }
}
