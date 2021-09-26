using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherSample.Application.Weather.Services
{
    public interface IWeatherUpdate
    {
        void UpdateWeather(string cityName);
    }

}
