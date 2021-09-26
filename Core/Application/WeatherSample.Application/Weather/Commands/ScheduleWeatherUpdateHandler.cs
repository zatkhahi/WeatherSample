using Hangfire;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WeatherSample.Application.Weather.Services;

namespace WeatherSample.Application.Weather.Commands
{
    public class ScheduleWeatherUpdateHandler : IRequestHandler<ScheduleWeatherUpdate, ScheduleWeatherUpdateResult>
    {
        private readonly IMediator mediator;

        public ScheduleWeatherUpdateHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<ScheduleWeatherUpdateResult> Handle(ScheduleWeatherUpdate request, CancellationToken cancellationToken)
        {
            if (request.ScheduleTime <= DateTime.Now)
                throw new InvalidOperationException("Schedule time cannot be less than now");

            var delay = request.ScheduleTime - DateTime.Now;

            var jobId = BackgroundJob.Schedule<IWeatherUpdate>(s => s.UpdateWeather(request.CityName),
                delay);

            return new ScheduleWeatherUpdateResult()
            {
                JobId = jobId
            };
        }
    }
}
