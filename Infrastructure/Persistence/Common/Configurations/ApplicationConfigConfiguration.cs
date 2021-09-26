using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    public class ApplicationConfigConfiguration : IEntityTypeConfiguration<ApplicationConfig>
    {
        public void Configure(EntityTypeBuilder<ApplicationConfig> builder)
        {
            builder.Property(s => s.Key).HasMaxLength(255); // .UseCollation("utf8_unicode_ci");
            builder.HasKey(x => x.Key);
            //builder.HasIndex(x => x.Id).IsUnique();

        }
    }
}
