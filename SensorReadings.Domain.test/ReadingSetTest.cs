﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SensorReadings.Domain.Test
{
    public class ReadingSetTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReadingSet_ConstructorCreatesObjectWithInitializedCollections()
        {
            //Arrange

            //Act
            var reading = new ReadingSet();

            //Assert
            Assert.IsNotNull(reading.Temperatures);
            Assert.IsNotNull(reading.Humidities);
            Assert.IsNotNull(reading.Rainfalls);
        }

        [Test]
        public void ReadingSet_Contains_TemperatureHumidityAndRainFall()
        {
            //Arrange

            //Act
            var reading = new ReadingSet();

            //Assert
            Assert.IsInstanceOf<IEnumerable<Reading>>(reading.Temperatures);
            Assert.IsInstanceOf<IEnumerable<Reading>>(reading.Humidities);
            Assert.IsInstanceOf<IEnumerable<Reading>>(reading.Rainfalls);
        }

        [Test]
        public void ReadingsSet_Resturns_TemperatureReadings()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}
