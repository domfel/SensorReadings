using SensorReadings.Domain;
using System;

namespace SensorReadings.Application.Models
{
    public class MeasurementsResponse
    {

        public MeasurementsResponse()
        {

        }
        public MeasurementsResponse(string deviceName, DateTime measurementDate, ReadingSet readings)
        {
            DeviceName = deviceName;
            MeasurementDate = measurementDate;
            Readings = readings;
        }

        public string DeviceName { get; }
        public DateTime MeasurementDate { get; }
        public ReadingSet Readings { get; }
    }
}
