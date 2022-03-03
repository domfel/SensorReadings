using SensorReadings.Utilities;
using System;
using SensorReadings.Application.Models;
using System.Threading.Tasks;
using SensorReadings.Domain;

namespace SensorReadings.Application
{
    public class GetMeasurementsQueryHandler : IQueryHandler<GetMeasurementsQuery, MeasurementsResponse>
    {
        private readonly IReadingArchive _readingArchive;

        public GetMeasurementsQueryHandler(
            IReadingArchive readingArchive)
        {
            _readingArchive = readingArchive;
        }

        public async Task<MeasurementsResponse> Execute(GetMeasurementsQuery query)
        {
            var date = DateTime.Parse(query.MeasurementDate);

            _readingArchive.InitializeArchive(query.DeviceName, date);
            await _readingArchive.SetReadingsAsync();
            var readings = await _readingArchive.GetReadingsAsync();

            return await Task.FromResult(new MeasurementsResponse(query.DeviceName, date, readings));
        }
    }
}
