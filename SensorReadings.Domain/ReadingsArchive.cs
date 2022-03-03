
using SensorReadings.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SensorReadings.Domain
{
    public class ReadingsArchive : IReadingArchive
    {
        private readonly IStorageRepository _storageRepository;
        public string DeviceId { get; private set; }
        public DateTime ReadingDate { get; private set; }
        private ReadingSet _readings;

        public ReadingsArchive(IStorageRepository storageRepository) 
        {           
            _storageRepository = storageRepository;
        }

        public void InitializeArchive(string deviceId, DateTime readingDate)
        {
            DeviceId = deviceId;
            ReadingDate = readingDate;
            _readings = new ReadingSet();
        }

        public async Task SetReadingByTypeAsync(ReadingType readingType)
        {
            var fileType =  await _storageRepository.IsHitoricData(DeviceId, ReadingDate, readingType);
            
            switch (fileType)
            {
                case DataState.Historic:
                    SetReadings(readingType, await _storageRepository.GetMeasurementFromZip(DeviceId, ReadingDate, readingType));
                    break;
                case DataState.Recent:
                    SetReadings(readingType, await _storageRepository.GetMeasurementFromCsvFile(DeviceId, ReadingDate, readingType));
                    break;
                default:
                    SetReadings(readingType, await Task.FromResult(new List<Reading>()));
                    break;
            }
        }

        public async Task SetReadingsAsync()
        {
            var fileType = await _storageRepository.IsHitoricData(DeviceId, ReadingDate);
            switch (fileType)
            {
                case DataState.Historic:
                    _readings = await _storageRepository.GetAllMeasurementsFromZipStorage(DeviceId, ReadingDate);
                    break;
                case DataState.Recent:
                    _readings = await _storageRepository.GetAllMeasurementsFromCsvFile(DeviceId, ReadingDate);
                    break;
                default:
                    _readings = new ReadingSet();
                    break;
            }
        }

        public async Task<ReadingSet> GetReadingsAsync()
        {
            return await Task.FromResult(_readings);
        }

        private void SetReadings(ReadingType type, IEnumerable<Reading> readings)
        {
            switch (type)
            {
                case ReadingType.Humidity:
                    _readings.Humidities = readings;
                    break;
                case ReadingType.Temperature:
                    _readings.Temperatures = readings;
                    break;
                case ReadingType.Rainfall:
                    _readings.Rainfalls = readings;
                        break;
                default:
                    break;
            }
        }
    }
}
