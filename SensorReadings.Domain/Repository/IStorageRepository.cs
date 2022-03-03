using SensorReadings.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SensorReadings.Domain.Repository
{
    public interface IStorageRepository
    {
        Task<DataState> IsHitoricData(string deviceId, DateTime date, ReadingType readingType = ReadingType.Humidity);
        Task<ReadingSet> GetAllMeasurementsFromCsvFile(string deviceId, DateTime date);
        Task<ReadingSet> GetAllMeasurementsFromZipStorage(string deviceId, DateTime date);
        Task<IEnumerable<Reading>> GetMeasurementFromCsvFile(string deviceId, DateTime date, ReadingType reading);
        Task<IEnumerable<Reading>> GetMeasurementFromZip(string deviceId, DateTime date, ReadingType reading);
    }
}