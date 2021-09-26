using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configurations
{
    public class UserScopeConfiguration : IEntityTypeConfiguration<UserScope>
    {
        public void Configure(EntityTypeBuilder<UserScope> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).UseIdentityColumn();

            builder.Property(s => s.ScopeName).HasMaxLength(150);
            builder.Property(x => x.Operation).HasMaxLength(50);
            builder.Property(x => x.EntityKey).HasMaxLength(150);

            builder.HasIndex(s => s.UserId);
            builder.HasIndex(s => new { s.UserId, s.ScopeName });
        }
    }
}
