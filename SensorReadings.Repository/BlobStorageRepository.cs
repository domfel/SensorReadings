using System;
using System.IO.Compression;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using SensorReadings.Domain;
using SensorReadings.Domain.Repository;

namespace SensorReadings.Repository
{
    public class BlobStorageRepository : IStorageRepository
    {
        private readonly BlobServiceClient _client;
        private readonly BlobContainerClient _container;

        public BlobStorageRepository(string connectionString, string containerName)
        {
            _client = new BlobServiceClient(connectionString);
            _container = _client.GetBlobContainerClient(containerName);
        }

        public async Task<ReadingSet> GetAllMeasurementsFromCsvFile(string deviceId, DateTime date)
        {

            var blobClients = new Dictionary<ReadingType, BlobClient>
            {
                { ReadingType.Humidity, _container.GetBlobClient(GetFileName(deviceId, ReadingType.Humidity, date)) },
                { ReadingType.Rainfall, _container.GetBlobClient(GetFileName(deviceId, ReadingType.Rainfall, date)) },
                { ReadingType.Temperature, _container.GetBlobClient(GetFileName(deviceId, ReadingType.Temperature, date)) }
            };

            var tasks = blobClients.
                Select(x => Task.Run(() => ReadBlobCsv(x)))
                .ToArray();

            Task.WaitAll(tasks);

            var readingSet = await CreateReadingSet(tasks);

            return await Task.FromResult(readingSet);
        }

        public async Task<ReadingSet> GetAllMeasurementsFromZipStorage(string deviceId, DateTime date)
        {
            var blobClients = new Dictionary<ReadingType, BlobClient>
            {
                { ReadingType.Humidity, _container.GetBlobClient(GetZIPFileName(deviceId, ReadingType.Humidity)) },
                { ReadingType.Rainfall, _container.GetBlobClient(GetZIPFileName(deviceId, ReadingType.Rainfall)) },
                { ReadingType.Temperature, _container.GetBlobClient(GetZIPFileName(deviceId, ReadingType.Temperature)) }
            };

            var tasks = blobClients.Select(x => Task.Run(() => ReadCsvFromFile(x, date))).ToArray();

            Task.WaitAll(tasks);
            var readingSet = await CreateReadingSet(tasks);


            return await Task.FromResult(readingSet);
        }

        public async Task<IEnumerable<Reading>> GetMeasurementFromCsvFile(string deviceId, DateTime date, ReadingType reading)
        {
            var blobClient = new KeyValuePair<ReadingType, BlobClient>(reading, _container.GetBlobClient(GetFileName(deviceId, reading, date)));

            var result = await ReadBlobCsv(blobClient);

            return await Task.FromResult(result.readings);
        }

        public async Task<IEnumerable<Reading>> GetMeasurementFromZip(string deviceId, DateTime date, ReadingType reading)
        {

            var blobClient = new KeyValuePair<ReadingType, BlobClient>(reading, _container.GetBlobClient(GetZIPFileName(deviceId, reading)));

            var result = await ReadCsvFromFile(blobClient, date);

            return await Task.FromResult(result.readings);
        }

        private static async Task<(ReadingType rt, List<Reading> readings)> ReadBlobCsv(KeyValuePair<ReadingType, BlobClient> entry)
        {
            var blobClient = entry.Value;

            var readings = new List<Reading>();
            using var stream = await blobClient.OpenReadAsync();
            using var sr = new StreamReader(stream);
            while (!sr.EndOfStream)
            {
                var fileLine = sr.ReadLine();
                var reading = fileLine.Split(';');
                readings.Add(new Reading(DateTime.Parse(reading[0]), reading[1]));
            }
            return await Task.FromResult((entry.Key, readings));
        }

        private static async Task<(ReadingType rt, List<Reading> readings)> ReadCsvFromFile(KeyValuePair<ReadingType, BlobClient> entry, DateTime date)
        {
            var blobClient = entry.Value;

            var readings = new List<Reading>();
            using var stream = await blobClient.OpenReadAsync();
            using ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Read);
            var zipEntry = zip.GetEntry($"{date:yyyy-MM-dd}.csv");
            using var zipStraem = zipEntry.Open();

            using var sr = new StreamReader(zipStraem);
            while (!sr.EndOfStream)
            {
                var fileLine = sr.ReadLine();
                var reading = fileLine.Split(';');
                readings.Add(new Reading(DateTime.Parse(reading[0]), reading[1]));
            }
            return await Task.FromResult((entry.Key, readings));
        }

        public async Task<DataState> IsHitoricData(string deviceId, DateTime date, ReadingType readingType = ReadingType.Humidity)
        {
            var csvBlobClient = _container.GetBlobClient(GetFileName(deviceId, readingType, date));
            if (await csvBlobClient.ExistsAsync())
            {
                return await Task.FromResult(DataState.Recent);
            }
            var zipFileBlobClient = _container.GetBlobClient(GetZIPFileName(deviceId, readingType));
            if (await zipFileBlobClient.ExistsAsync())
            {
                using var stream = await zipFileBlobClient.OpenReadAsync();
                using ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Read);
                try
                {
                    var exists = zip.GetEntry($"{date:yyyy-MM-dd}.csv");
                    if (exists != null)
                    {
                        return await Task.FromResult(DataState.Historic);
                    }
                }
                catch (Exception)
                {
                    return await Task.FromResult(DataState.NotFound);
                }

            }
            return await Task.FromResult(DataState.NotFound);
        }

        private static async Task<ReadingSet> CreateReadingSet(Task<(ReadingType rt, List<Reading> readings)>[] tasks)
        {
            var readingSet = new ReadingSet();
            foreach (var task in tasks)
            {
                var result = await task;
                switch (result.rt)
                {
                    case ReadingType.Humidity:
                        readingSet.Humidities = result.readings;
                        break;
                    case ReadingType.Rainfall:
                        readingSet.Rainfalls = result.readings;
                        break;
                    case ReadingType.Temperature:
                        readingSet.Temperatures = result.readings;
                        break;
                }
            }
            return readingSet;
        }

        private string GetFileName(string deviceId, ReadingType readingType, DateTime date)
        {
            return $"{deviceId}/{readingType.ToString().ToLower()}/{date:yyyy-MM-dd}.csv";
        }
        private string GetZIPFileName(string deviceId, ReadingType readingType)
        {
            return $"{deviceId}/{readingType.ToString().ToLower()}/historical.zip";
        }
    }
}
