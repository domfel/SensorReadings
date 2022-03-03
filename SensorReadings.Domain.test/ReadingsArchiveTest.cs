using Moq;
using NUnit.Framework;
using SensorReadings.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensorReadings.Domain.test
{
    public class ReadingsArchiveTest
    {
        private const string _deviceId = "123321";
        private readonly DateTime _date = DateTime.Now;
        private readonly ReadingType _readingType = ReadingType.Humidity;
        private Mock<IStorageRepository> _storage;
        private ReadingsArchive _archive;
        private ReadingSet _readingSet;

        [OneTimeSetUp]
        public void OnTimeSetup()
        {
            _readingSet = new ReadingSet()
            {
                Humidities = new List<Reading>() { new Reading(DateTime.Now, "-0,87") },
                Rainfalls = new List<Reading>() { new Reading(DateTime.Now, "-0,87") },
                Temperatures = new List<Reading>() { new Reading(DateTime.Now, "-0,87") }
            };
        }
        
        [SetUp]
        public void Setup()
        {
            _storage = new Mock<IStorageRepository>();
            _archive = new ReadingsArchive(_storage.Object);            
        }

        [Test]
        public void InitializeArchive_InitializesArchiveWithSetupDeviceIdAndDateAndReadingSet()
        {
            //Arrange
            

            //Act
            _archive.InitializeArchive(_deviceId, _date);

            //Assert
            Assert.IsNotNull(_archive);
            Assert.AreEqual(_deviceId, _archive.DeviceId);
            Assert.AreEqual(_date, _archive.ReadingDate);
            Assert.IsInstanceOf<string>(_archive.DeviceId);
            Assert.IsInstanceOf<DateTime>(_archive.ReadingDate);            
        }

        [Test]
        public async Task SetReadingByTypeAsync_ObtainsDataAndSetupsReadingSetFromCSVFile()
        {
            //Arrange
            _storage.Setup(x => x.IsHitoricData(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<ReadingType>()))
            .ReturnsAsync(DataState.Recent);

            _storage.Setup(x => x.GetMeasurementFromCsvFile(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<ReadingType>()))
                .ReturnsAsync(new List<Reading>() { new Reading(DateTime.Now, "-0,87") });

            //Act
            _archive.InitializeArchive(_deviceId, _date);
            await _archive.SetReadingByTypeAsync(_readingType);
            var result = await _archive.GetReadingsAsync();

            //Assert
            Assert.IsNotNull(result);
            Assert.Greater(result.Humidities.Count(), 0);
            _storage.Verify();
        }

        [Test]
        public async Task SetReadingByTypeAsync_ObtainsDataAndSetupsReadingSetFromZipFile()
        {
            //Arrange
            _storage.Setup(x => x.IsHitoricData(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<ReadingType>()))
            .ReturnsAsync(DataState.Historic);

            _storage.Setup(x => x.GetMeasurementFromZip(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<ReadingType>()))
                .ReturnsAsync(new List<Reading>() { new Reading(DateTime.Now, "-0,87") });

            //Act
            _archive.InitializeArchive(_deviceId, _date);
            await _archive.SetReadingByTypeAsync(_readingType);
            var result = await _archive.GetReadingsAsync();

            //Assert
            Assert.IsNotNull(result);
            Assert.Greater(result.Humidities.Count(), 0);
            _storage.Verify();
        }

        [Test]
        public async Task SetReadingsAsync_ObtainsDataAndSetupsReadingSetFromZipFile()
        {
            //Arrange
            _storage.Setup(x => x.IsHitoricData(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<ReadingType>()))
            .ReturnsAsync(DataState.Historic);
            _storage.Setup(x => x.GetAllMeasurementsFromZipStorage(It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync(_readingSet);

            //Act
            _archive.InitializeArchive(_deviceId, _date);
            await _archive.SetReadingsAsync();
            var result = await _archive.GetReadingsAsync();

            //Assert
            Assert.IsNotNull(result);
            Assert.Greater(result.Humidities.Count(), 0);
            Assert.Greater(result.Rainfalls.Count(), 0);
            Assert.Greater(result.Temperatures.Count(), 0);
            _storage.Verify();
        }

        [Test]
        public async Task SetReadingsAsync_ObtainsDataAndSetupsReadingSetFromCsvFile()
        {
            //Arrange
            _storage.Setup(x => x.IsHitoricData(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<ReadingType>()))
            .ReturnsAsync(DataState.Recent);
            _storage.Setup(x => x.GetAllMeasurementsFromCsvFile(It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync(_readingSet);

            //Act
            _archive.InitializeArchive(_deviceId, _date);
            await _archive.SetReadingsAsync();
            var result = await _archive.GetReadingsAsync();

            //Assert
            Assert.IsNotNull(result);
            Assert.Greater(result.Humidities.Count(), 0);
            Assert.Greater(result.Rainfalls.Count(), 0);
            Assert.Greater(result.Temperatures.Count(), 0);
            _storage.Verify();
        }
    }
}