using System;
using System.Collections.Generic;
using System.Text;

namespace SensorReadings.Domain
{
    public class Reading
    {
        private DateTime _t;
        public string T { get => _t.ToString("HH:mm:ss"); }
        public string V { get; private set; }

        public Reading(DateTime measuredAt, string measuredValue)
        {
            _t = measuredAt;
            V = measuredValue;
        }
    }
}
