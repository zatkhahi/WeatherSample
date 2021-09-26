using Common.Presentation.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WeatherSample.Application.Weather.Commands;

namespace WeatherSample.Controllers
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : BaseController
    {
        [HttpPost("ScheduleWeatherUpdate")]
        public async Task<IActionResult> ScheduleWeatherUpdate(ScheduleWeatherUpdate command)
        {
            return await HandleRequest(command);
        }
    }
}
