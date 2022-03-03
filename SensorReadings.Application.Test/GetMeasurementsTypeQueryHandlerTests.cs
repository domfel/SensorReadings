using Moq;
using NUnit.Framework;
using SensorReadings.Application.Models;
using SensorReadings.Domain;
using SensorReadings.Domain.Repository;
using SensorReadings.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SensorReadings.Application.Test
{
    public class GetMeasurementsTypeQueryHandlerTests
    {
        private IQueryHandler<GetMeasurementTypeQuery, MeasurementsResponse> _handler;
        private Mock<IReadingArchive> _archive;
        public const string _deviceName = "123";
        public readonly DateTime _date = DateTime.Parse("2020-10-01");
        public readonly ReadingType _type = ReadingType.Humidity;

        [SetUp]
        public void Setup()
        {
            _archive = new Mock<IReadingArchive>();
            _handler = new GetMeasurementTypeQueryHandler(_archive.Object);
        }

        [Test]
        public async Task Execute_InitializesReadingArchive()
        {
            //Arragne            
            var input = new GetMeasurementTypeQuery(_deviceName, _date.ToString(), _type.ToString());
            _archive.Setup(x => x.InitializeArchive(It.Is<string>(p => p == _deviceName), It.Is<DateTime>(p => p == _date)))
                .Verifiable();

            //Act
            var result = await _handler.Execute(input);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<MeasurementsResponse>(result);
            _archive.Verify();
        }

        [Test]
        public async Task Execute_GetDataFromReadingArchive_ReturnsData()
        {
            //Arragne 
            var readings = new List<Reading>() { new Reading(DateTime.Now, "-0,5") };
            var input = new GetMeasurementTypeQuery(_deviceName, _date.ToString(), _type.ToString());
            _archive.Setup(x => x.SetReadingByTypeAsync(It.Is<ReadingType>(p => p == _type)))
                .Verifiable();

            //Act
            var result = await _handler.Execute(input);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<MeasurementsResponse>(result);
            _archive.Verify();
        }
    }
}