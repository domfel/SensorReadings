# SensorReadings

This service exposes REST API for obtaining sernos readings arhivied in the cloud storage

## Quick Start guide
1. Set proper `"BlobStorageConnectionString"` and `"AzureStorageContainer"` values in *local.settings.json* file. (originals removed because of placing in public repo)
2. Build and run solution


## API reference
API exposes two endpoints which allows for obtainig sensor readings from storage arhive. First endpoint returns all types of measurements from specific device and date, second allows to specify also type of measurement which will be returned.

### Get all readings
Reaturn all readings from specific device and date. Endpoint url: `{baseUrl}/api/device/{deviceId}/date/{readingsDate}`, where:  
- {baseUrl} - bese address of the endpoint
- {deviceId} - Id of the device which measurements will be returned
- {readingsDate} - Date from which the measirements will be retruned

Response format constains collection of pairs `"T","V"` where: 
- T - represnets time of mesurment in format `"HH:mm:ss"`
- V - value of the measurement
 
#### Sample reauest
GET http://localhost:7071/api/device/dockan/date/2015-10-02

#### Sample response
```json {
  "deviceName": "dockan",
  "measurementDate": "2015-10-02T00:00:00",
  "readings": {
    "temperatures": [
      {
        "t": "00:00:00",
        "v": "22,16"
      },
      {
        "t": "00:00:05",
        "v": "22,16"
      },
      [...]
    ],
    "humidities": [
      {
        "t": "00:00:00",
        "v": "47,41"
      },
      {
        "t": "00:00:05",
        "v": "47,41"
      }, 
      [...]
    ],
    "rainfalls": [
      {
        "t": "00:00:00",
        "v": ",00"
      },
      {
        "t": "00:00:05",
        "v": ",00"
      },
      [...]
    ]
  }
} 
```


### Get readings by reading type
Reaturn readings from specific devic, date and for specified readings type. Endpoint url: `{baseUrl}/api/device/{deviceId}/date/{readingsDate}/measurementtype/{readingType}}`, where:  
- {baseUrl} - bese address of the endpoint
- {deviceId} - Id of the device which measurements will be returned
- {readingsDate} - Date from which the measirements will be retruned
- {readingType} - type of reading to equire (allowed types: Temperature, Humidity, Rainfall)

Response format constains collection of pairs `"T","V"` where: 
- T - represnets time of mesurment in format `"HH:mm:ss"`
- V - value of the measurement

#### Sample reauest
GET  http://localhost:7071/api/device/dockan/date/2015-10-01/measurementtype/Temperature

#### Sample response
```json {
  "deviceName": "dockan",
  "measurementDate": "2015-10-01T00:00:00",
  "readings": {
    "temperatures": [
      {
        "t": "00:00:00",
        "v": "22,33"
      },
      {
        "t": "00:00:05",
        "v": "22,33"
      },
      [...]
    ],
    "humidities": [],
    "rainfalls": []
  }
}
```