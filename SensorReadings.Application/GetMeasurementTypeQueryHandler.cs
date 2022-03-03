using System;
using System.Threading.Tasks;
using SensorReadings.Application.Models;
using SensorReadings.Domain;
using SensorReadings.Utilities;

namespace SensorReadings.Application
{
    public class GetMeasurementTypeQueryHandler : IQueryHandler<GetMeasurementTypeQuery, MeasurementsResponse>
    {
        private readonly IReadingArchive _readingArchive;

        public GetMeasurementTypeQueryHandler(IReadingArchive readingArchive)
        {
            _readingArchive = readingArchive;
        }

        public async Task<MeasurementsResponse> Execute(GetMeasurementTypeQuery query)
        {
            var date = DateTime.Parse(query.MeasurementDate);
            Enum.TryParse<ReadingType>(query.MeasurementType, out var measurementType);

            _readingArchive.InitializeArchive(query.DeviceName, date);
            await _readingArchive.SetReadingByTypeAsync(measurementType);
            var readings = await _readingArchive.GetReadingsAsync();

            return await Task.FromResult(new MeasurementsResponse(query.DeviceName, date, readings));
        }
    }
}
