using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherSample.Domain;

namespace WeatherSample.Application.Interfaces
{
    public interface IWeatherDbContext : ICommonDbContext
    {
        DbSet<CityWeatherData> CityWeatherDatas { get; set; }
    }
}
