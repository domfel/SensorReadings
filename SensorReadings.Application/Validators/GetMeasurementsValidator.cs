using FluentValidation;
using SensorReadings.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SensorReadings.Application.Validators
{
    public class GetMeasurementsValidator : AbstractValidator<GetMeasurementsQuery>
    {
        public GetMeasurementsValidator()
        {
            RuleFor(x => x.MeasurementDate)                
                .NotEmpty()
                .Must(f => DateTime.TryParse(f, out var output));


            RuleFor(x => x.DeviceName)                
                .NotEmpty();                           
        }
    }
}
