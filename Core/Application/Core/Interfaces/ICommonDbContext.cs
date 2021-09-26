using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICommonDbContext
    {
        DbSet<ApplicationUser> Users { get; }
        DbSet<ApplicationUserClaim> UserClaims { get; set; }
        DbSet<IdentityUserLogin<int>> UserLogins { get; set; }
        DbSet<IdentityUserToken<int>> UserTokens { get; set; }
        DbSet<ApplicationUserRole> UserRoles { get; set; }
        DbSet<ApplicationRole> Roles { get; set; }
        DbSet<IdentityRoleClaim<int>> RoleClaims { get; set; }
        DbSet<UserScope> UserScopes { get; set; }
        DbSet<RoleScope> RoleScopes { get; set; }
        DbSet<ApplicationScope> ApplicationScopes { get; set; }
        DbSet<ScopeOperation> ScopeOperations { get; set; }

        DbSet<ApplicationConfig> ApplicationConfigs { get; set; }

        DbSet<Job> Jobs { get; set; } 
        


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
        Task<int> ExecuteSqlCommandAsync(FormattableString sql, CancellationToken cancellationToken = default);
        Task ExecuteSqlQueryAsync(Action<DbDataReader> read, string sql, CancellationToken cancellationToken = default, params object[] parameters);
        IDbContextTransaction BeginTransaction();
        EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;
        EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class;
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        ChangeTracker ChangeTracker { get; }
    }
}
