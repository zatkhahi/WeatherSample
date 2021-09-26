using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WeatherSample.Domain;

namespace Persistence.Configurations
{
    public class CityWeatherDataConfiguration : IEntityTypeConfiguration<CityWeatherData>
    {
        public void Configure(EntityTypeBuilder<CityWeatherData> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).UseIdentityColumn();

            builder.Property(s => s.Temperature).HasPrecision(8, 2);

            builder.Property(s => s.CityName).HasMaxLength(255);
            builder.HasIndex(s => s.CityName);

        }
    }
}
