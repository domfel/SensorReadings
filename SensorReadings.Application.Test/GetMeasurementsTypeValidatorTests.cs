using NUnit.Framework;
using SensorReadings.Application.Models;
using SensorReadings.Application.Validators;

namespace SensorReadings.Application.Test
{
    public class GetMeasurementsTypeValidatorTests
    {
        private GetMeasurementTypeValidator _validator;
        private GetMeasurementsValidator _baseValidator;

        [SetUp]
        public void Setup()
        {
            _baseValidator = new GetMeasurementsValidator();
            _validator = new GetMeasurementTypeValidator(_baseValidator);
        }

        [TestCase("Id", "2010-10-01", "Temperature", true, TestName = "Validate_ReturnsValidWhenPassingTemperatureAndValidParams")]
        [TestCase("Id", "2010-10-01", "Humidity", true, TestName = "Validate_ReturnsValidWhenHumidityAndPassingValidParams")]
        [TestCase("Id", "2010-10-01", "Rainfall", true, TestName = "Validate_ReturnsValidWhenRainFallAndPassingValidParams")]
        [TestCase("Id", "2010-10-01", "", false, TestName = "Validate_ReturnsValidWhenTypeEmpty")]
        [TestCase("Id", "2010-10-01", "xyz", false, TestName = "Validate_ReturnsValidWhenTypeEmpty")]
        public void GetMeasurementsValidator_Validate(string name, string date, string type, bool validationResult)
        {
            //Arragne            
            var input = new GetMeasurementTypeQuery(name, date, type);

            //Act
            var result = _validator.Validate(input);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(validationResult, result.IsValid);
        }
    }
}