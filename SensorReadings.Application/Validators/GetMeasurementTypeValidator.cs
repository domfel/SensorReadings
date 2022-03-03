using FluentValidation;
using SensorReadings.Application.Models;
using SensorReadings.Domain;

namespace SensorReadings.Application.Validators
{
    public class GetMeasurementTypeValidator : AbstractValidator<GetMeasurementTypeQuery>
    {
        public GetMeasurementTypeValidator(AbstractValidator<GetMeasurementsQuery> baseValidator)
        {
            Include(baseValidator);
            
            RuleFor(x => x.MeasurementType)               
                .IsEnumName(typeof(ReadingType));
        }
    }
}
