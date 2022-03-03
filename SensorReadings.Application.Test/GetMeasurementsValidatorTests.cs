using NUnit.Framework;
using SensorReadings.Application.Models;
using SensorReadings.Application.Validators;

namespace SensorReadings.Application.Test
{
    public class GetMeasurementsValidatorTests
    {
        private GetMeasurementsValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new GetMeasurementsValidator();
        }

        [TestCase("Id", "2010-10-01", true, TestName = "Validate_ReturnsValidWhenPassingValidParams")]
        [TestCase("", "2010-10-01", false, TestName = "Validate_ReturnsNotValidWhenPassingEmptyId")]
        [TestCase(null, "2010-10-01", false, TestName = "Validate_ReturnsValidWhenParamsNullId")]
        [TestCase("Id", "", false, TestName = "Validate_ReturnsValidWhenPassingEmptyDate")]
        [TestCase("Id", null, false, TestName = "Validate_ReturnsValidWhenPassingNullDate")]
        [TestCase("Id", "aaaaaaa", false, TestName = "Validate_ReturnsValidWhenPassingInvalkidDateFormat")]
        public void GetMeasurementsValidator_Validate(string name, string date, bool validationResult)
        {
            //Arragne            
            var input = new GetMeasurementsQuery(name, date);

            //Act
            var result = _validator.Validate(input);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(validationResult, result.IsValid);
        }
    }
}