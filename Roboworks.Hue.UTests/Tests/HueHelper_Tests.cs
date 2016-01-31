using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Roboworks.Hue.UTests.Tests
{
    [TestClass]
    public class HueHelper_Tests
    {
        [TestMethod]
        public void Response_error_checking_throws_hue_exception_with_error_type_parsed_from_passed_data()
        {
            // Arrange
            HueApiResponseErrorException exception = null;

            // Act
            try
            {
                HueHelper.HueApiResponseErrorCheck(TestData.HueApiResponse_LinkButtonNotPressedError);
            }
            catch(HueApiResponseErrorException ex)
            {
                exception = ex;
            }

            // Assert
            Assert.AreEqual(HueErrorType.LinkButtonNotPressed, exception.ErrorType);
        }

        [TestMethod]
        public void Response_success_delete_checking_does_not_throw_exception_when_passed_data_contains_resource_deletion_confirmation()
        {
            // Arrange
            Exception exception = null;

            // Act
            try
            {
                HueHelper.HueApiResponseSuccessDeleteCheck(
                    TestData.HueUserDelete_Response, 
                    TestData.HueUser_ResourceLocation
                );
            }
            catch(Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.IsNull(exception);
        }

        [TestMethod]
        public void Response_success_delete_checking_throws_exception_when_passed_data_does_not_contain_resource_deletion_confirmation()
        {
            // Arrange
            Exception exception = null;

            // Act
            try
            {
                HueHelper.HueApiResponseSuccessDeleteCheck(
                    TestData.HueApiResponse_ResourceIsNotAvailableError,
                    TestData.HueUser_ResourceLocation
                );
            }
            catch(Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.IsNotNull(exception);
        }
    }
}
