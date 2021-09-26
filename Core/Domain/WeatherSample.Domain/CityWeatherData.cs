using System;

namespace WeatherSample.Domain
{
    public class CityWeatherData
    {
        public long Id { get; set; }
        public string CityName { get; set; }
        public DateTime RecordTime { get; set; }
        public decimal Temperature { get; set; }
    }
}
