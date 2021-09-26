using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    public class ApplicationScopeConfiguration : IEntityTypeConfiguration<ApplicationScope>
    {
        public void Configure(EntityTypeBuilder<ApplicationScope> builder)
        {
            builder.HasKey(s => s.Name);
            builder.Property(x => x.Name).HasMaxLength(150);

            //Relations
            builder.HasMany(x => x.Operations).WithOne(r => r.Scope).HasForeignKey(r => r.ScopeName).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
