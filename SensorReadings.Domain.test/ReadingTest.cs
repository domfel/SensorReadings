using NUnit.Framework;
using System;

namespace SensorReadings.Domain.Test
{
    public class ReadingTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Reading_Contains_DateAndReading()
        {
            //Arrange

            //Act
            var reading = new Reading(DateTime.Now, "123,123");

            //Assert
            Assert.IsInstanceOf<DateTime>(reading.d);
            Assert.IsInstanceOf<string>(reading.v);
        }
    }
}
