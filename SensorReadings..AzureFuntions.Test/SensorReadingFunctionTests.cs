using FluentValidation;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using SensorReadings.Application.Models;
using SensorReadings.Utilities;
using System.Threading.Tasks;

namespace SensorReadings.AzureFunctions.Test
{
    public class SensorReadingFunctionTests
    {
        private Mock<HttpRequest> _requestMock;
        private Mock<RequestExecutor<GetMeasurementsQuery, MeasurementsResponse>> _executor;
        private Mock<IQueryHandler<GetMeasurementsQuery, MeasurementsResponse>> _queryHandler;
        private Mock<AbstractValidator<GetMeasurementsQuery>> _validator;

        [SetUp]
        public void Setup()
        {
            _requestMock = new Mock<HttpRequest>();
            _queryHandler = new Mock<IQueryHandler<GetMeasurementsQuery, MeasurementsResponse>>();
            _validator = new Mock<AbstractValidator<GetMeasurementsQuery>>();
            _executor = new Mock<RequestExecutor<GetMeasurementsQuery, MeasurementsResponse>>(_validator.Object, _queryHandler.Object);
        }

        [Test]
        public async Task GetSensorReadings_Returns_Success()
        {
            //Arrange

            //Act
            var p = await SensorReadingFunction.GetSensorReadings(_requestMock.Object, "de", "f", _executor.Object);

            //Assert
            
        }
    }
}