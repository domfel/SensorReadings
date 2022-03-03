namespace SensorReadings.Application.Models
{
    public class GetMeasurementTypeQuery : GetMeasurementsQuery
    {
        public string MeasurementType { get; set; }

        public GetMeasurementTypeQuery(string deviceName, string measurementDate, string measurementType)
            :base(deviceName,measurementDate)
        {
            MeasurementType = measurementType;
        }
    }
}
