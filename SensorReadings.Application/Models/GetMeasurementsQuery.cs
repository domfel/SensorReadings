namespace SensorReadings.Application.Models
{
    public class GetMeasurementsQuery
    {
        public GetMeasurementsQuery(string deviceName, string measurementDate)
        {
            DeviceName = deviceName;
            MeasurementDate = measurementDate;
        }

        public string DeviceName { get; }
        public string MeasurementDate { get; }
    }
}
