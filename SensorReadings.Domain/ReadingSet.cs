using System;
using System.Collections.Generic;

namespace SensorReadings.Domain
{
    public class ReadingSet
    {
        public IEnumerable<Reading> Temperatures { get; set; }
        public IEnumerable<Reading> Humidities { get; set; }
        public IEnumerable<Reading> Rainfalls { get; set; }

        public ReadingSet()
        {
            Temperatures = new List<Reading>();
            Humidities = new List<Reading>();
            Rainfalls = new List<Reading>();
        }
    }
}
