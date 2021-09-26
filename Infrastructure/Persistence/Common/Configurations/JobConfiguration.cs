using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasKey(x => x.Id);
            //builder.HasIndex(x => x.Id).IsUnique();

            builder.HasMany(s => s.JobLogs).WithOne(s => s.Job).HasForeignKey(s => s.JobId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
