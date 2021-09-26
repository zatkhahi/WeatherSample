using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherSample.Application.Weather.Commands
{
    public class ScheduleWeatherUpdate : IRequest<ScheduleWeatherUpdateResult>
    {
        public DateTime ScheduleTime { get; set; }
        public string CityName { get; set; }
    }

    public class ScheduleWeatherUpdateResult
    {
        public string JobId { get; set; }
    }
}
