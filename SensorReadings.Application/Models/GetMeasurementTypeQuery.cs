namespace SensorReadings.Application.Models
{
    public class GetMeasurementTypeQuery : GetMeasurementsQuery
    {
        public GetMeasurementTypeQuery(string deviceName, string measurementDate, string measurementType)
            :base(deviceName,measurementDate)
        {
            MeasurementType = measurementType;
        }

        public string MeasurementType { get; set; }
    }
}
