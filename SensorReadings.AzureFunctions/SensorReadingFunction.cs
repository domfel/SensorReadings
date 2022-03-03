using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureFunctions.Autofac;
using SensorReadings.Application.Models;
using SensorReadings.Utilities;

namespace SensorReadings.AzureFunctions
{
    [DependencyInjectionConfig(typeof(AutofacConfig))]
    public static class SensorReadingFunction
    {
        [FunctionName(nameof(GetSensorReadings))]
        public static async Task<IActionResult> GetSensorReadings(
            [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "get",
            Route = "device/{deviceName}/date/{measurementDate}")] HttpRequest req,
            string deviceName,
            string measurementDate,
            [Inject] RequestExecutor<GetMeasurementsQuery, MeasurementsResponse> requestExecutor)
        {
            var input = new GetMeasurementsQuery(deviceName, measurementDate);
            return await requestExecutor.Execute(input);
        }

        [FunctionName(nameof(GetSensorReadingsByType))]
        public static async Task<IActionResult> GetSensorReadingsByType(
           [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "get",
            Route = "device/{deviceName}/date/{measurementDate}/measurementtype/{measurementType}")] HttpRequest req,
           string deviceName,
           string measurementDate,
           string measurementType,
           [Inject] RequestExecutor<GetMeasurementTypeQuery, MeasurementsResponse> requestExecutor
           )
        {
            var input = new GetMeasurementTypeQuery(deviceName, measurementDate, measurementType);
            return await requestExecutor.Execute(input);
        }
    }
}
