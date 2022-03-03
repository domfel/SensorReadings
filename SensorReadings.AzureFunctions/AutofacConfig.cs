using Autofac;
using AzureFunctions.Autofac.Configuration;
using AzureFunctions.Autofac.Shared.Extensions;
using FluentValidation;
using SensorReadings.Application;
using SensorReadings.Application.Models;
using SensorReadings.Application.Validators;
using SensorReadings.Domain;
using SensorReadings.Domain.Repository;
using SensorReadings.Repository;
using SensorReadings.Utilities;
using System;

namespace SensorReadings.AzureFunctions
{
    public class AutofacConfig
    {
        public AutofacConfig(string functionName)
        {
            DependencyInjection.Initialize(builder =>
            {
                builder.RegisterType<GetMeasurementsQueryHandler>().As<IQueryHandler<GetMeasurementsQuery, MeasurementsResponse>>();
                builder.RegisterType<GetMeasurementTypeQueryHandler>().As<IQueryHandler<GetMeasurementTypeQuery, MeasurementsResponse>>();
                builder.Register<AbstractValidator<GetMeasurementsQuery>>(c => new GetMeasurementsValidator());
                builder.Register<AbstractValidator<GetMeasurementTypeQuery>>(c=> new GetMeasurementTypeValidator(c.Resolve<AbstractValidator<GetMeasurementsQuery>>()));
                builder.Register(c => new RequestExecutor<GetMeasurementsQuery, MeasurementsResponse>(c.Resolve<AbstractValidator<GetMeasurementsQuery>>(),c.Resolve<IQueryHandler<GetMeasurementsQuery, MeasurementsResponse>>()));
                builder.Register(c=> new RequestExecutor<GetMeasurementTypeQuery, MeasurementsResponse>(c.Resolve<AbstractValidator<GetMeasurementTypeQuery>>(), c.Resolve<IQueryHandler<GetMeasurementTypeQuery, MeasurementsResponse>>()));
                builder.Register(c => new BlobStorageRepository(GetConfig("BlobStorageConnectionString"),GetConfig("AzureStorageContainer"))).As<IStorageRepository>();
                builder.RegisterType<ReadingsArchive>().As<IReadingArchive>();                
            }, functionName);
        }

        private string  GetConfig(string value)
        {
            return Environment.GetEnvironmentVariable(value, EnvironmentVariableTarget.Process);
        }
    }
}
