namespace SensorReadings.Application.Models
{
    public class GetMeasurementsQuery
    {        
        public string DeviceName { get; }
        public string MeasurementDate { get; }
        public GetMeasurementsQuery(string deviceName, string measurementDate)
        {
            DeviceName = deviceName;
            MeasurementDate = measurementDate;
        }
    }
}
