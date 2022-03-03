using Azure.Storage.Blobs;
using NUnit.Framework;
using Moq;
using System;
using System.Threading.Tasks;
using System.Linq;
using SensorReadings.Domain;
using System.Collections.Generic;

namespace SensorReadings.Repository.Test
{
    [Ignore("Requires valid connection string, can be mocked, but it requires more time")]    
    [TestFixture]
    public class BlobStorageRepositoryTest
    {        
        private BlobStorageRepository _repository;


        [SetUp]
        public void Setup()
        {   
            _repository = new BlobStorageRepository(
                "ConnectionString",
                "Container");
        }

        [Test]
        public async Task GetAllMeasurementsFromCsvFile_ReturnsAllMeasurements()
        {
            //Arnage                        
            var date = DateTime.Parse("2019/01/10");
            var deviceId = "dockan";

            //Act
            var result = await _repository.GetAllMeasurementsFromCsvFile(deviceId, date);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ReadingSet>(result);
            Assert.IsNotNull(result.Humidities);
            Assert.Greater(result.Humidities.Count(), 0);
            Assert.IsNotNull(result.Temperatures);
            Assert.Greater(result.Temperatures.Count(), 0);
            Assert.IsNotNull(result.Rainfalls);
            Assert.Greater(result.Rainfalls.Count(), 0);
        }

        [Test]
        public async Task CheckBlobFileType_IdentifiesCsvFile()
        {
            //Arnage                        
            var date = DateTime.Parse("2019/01/10");
            var deviceId = "dockan";

            //Act
            var result = await _repository.IsHitoricData(deviceId, date);

            //Asert
            Assert.IsInstanceOf<DataState>(result);
            Assert.AreEqual(DataState.Recent, result);
        }

        [Test]
        public async Task CheckBlobFileType_ReturnsZiPFile()
        {
            //Arnage
            var date = DateTime.Parse("2015/01/10");
            var deviceId = "dockan";

            //Act
            var result = await _repository.IsHitoricData(deviceId, date);

            //Asert
            Assert.IsInstanceOf<DataState>(result);
            Assert.AreEqual(DataState.Historic, result);
        }

        [Test]
        public async Task CheckBlobFileType_ReturnsNotFoundWhenDataIsNotPresentInStorage()
        {
            //Arnage          
            var date = DateTime.Parse("2000/01/10");
            var deviceId = "dockan";

            //Act
            var result = await _repository.IsHitoricData(deviceId, date);

            //Asert
            Assert.IsInstanceOf<DataState>(result);
            Assert.AreEqual(DataState.NotFound, result);
        }

        [Test]
        public async Task GetAllMeasurementsFromZipStorage_ReturnAllMeasurements()
        {
            //Arnage                      
            var date = DateTime.Parse("2015/01/10");
            var deviceId = "dockan";

            //Act
            var result = await _repository.GetAllMeasurementsFromZipStorage(deviceId, date);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ReadingSet>(result);
            Assert.IsNotNull(result.Humidities);
            Assert.Greater(result.Humidities.Count(), 0);
            Assert.IsNotNull(result.Temperatures);
            Assert.Greater(result.Temperatures.Count(), 0);
            Assert.IsNotNull(result.Rainfalls);
            Assert.Greater(result.Rainfalls.Count(), 0);
        }

        [Test]
        public async Task GetMeasurementFromCsvFile_ReturnMeasuremnt()
        {
            //Arrange           
            var date = DateTime.Parse("2019/01/10");
            var deviceId = "dockan";
            var reading = ReadingType.Temperature;

            //Act
            var result = await _repository.GetMeasurementFromCsvFile(deviceId, date, reading);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<Reading>>(result);           
            Assert.Greater(result.Count(), 0);
        }

        [Test]
        public async Task GetMeasurementFromZip_ReturnMeasuremnt()
        {
            //Arrange            
            var date = DateTime.Parse("2015/01/10");
            var deviceId = "dockan";
            var reading = ReadingType.Temperature;

            //Act
            var result = await _repository.GetMeasurementFromZip(deviceId, date, reading);

            //Assert
            Assert.IsNotNull(result);           
            Assert.IsInstanceOf<IEnumerable<Reading>>(result);
            Assert.Greater(result.Count(), 0);
        }
    }
}