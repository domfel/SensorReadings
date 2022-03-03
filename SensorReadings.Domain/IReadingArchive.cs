using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SensorReadings.Domain
{
    public interface IReadingArchive
    {
        void InitializeArchive(string deviceId, DateTime readingDate);
        Task SetReadingByTypeAsync(ReadingType readingType);
        Task SetReadingsAsync();
        Task<ReadingSet> GetReadingsAsync();
    }
}