using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    public class ScopeOperationConfiguration : IEntityTypeConfiguration<ScopeOperation>
    {
        public void Configure(EntityTypeBuilder<ScopeOperation> builder)
        {
            builder.HasKey(s => new { s.ScopeName, s.Operation });
            builder.Property(x => x.Operation).HasMaxLength(50);
        }
    }
}
