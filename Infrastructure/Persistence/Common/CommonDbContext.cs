using Application.Authorization;
using Application.Infrastructure;
using Application.Interfaces;
using Domain.Entities;
using Domain.SqlModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data.Common;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;


namespace Persistence
{
    public abstract class CommonDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int,
        ApplicationUserClaim, ApplicationUserRole, IdentityUserLogin<int>
        , IdentityRoleClaim<int>, IdentityUserToken<int>>, ICommonDbContext        
    {
        protected IPrincipal principal;
        public CommonDbContext(DbContextOptions options, IPrincipal principal = null)
           : base(options)
        {
            this.principal = principal;
        }
        //public IQueryable<ApplicationUser> UsersList => Users.AsQueryable<ApplicationUser>();            
        
        public DbSet<UserScope> UserScopes { get; set; }
        public DbSet<RoleScope> RoleScopes { get; set; }
        public DbSet<ApplicationScope> ApplicationScopes { get; set; }
        public DbSet<ScopeOperation> ScopeOperations { get; set; }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<ApplicationConfig> ApplicationConfigs { get; set; }
       


        public async Task<int> ExecuteSqlCommandAsync(FormattableString sql, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.Database.ExecuteSqlInterpolatedAsync(sql, cancellationToken);
        }

        public async Task ExecuteSqlQueryAsync(Action<DbDataReader> read, string sql, CancellationToken cancellationToken = default(CancellationToken), params object[] parameters)
        {
            object[] ps = parameters.Select(s =>
            {
                var newP = s is SqlQueryParameter ?
                    new SqlParameter()
                    {
                        ParameterName = (s as SqlQueryParameter).ParameterName,
                        Value = (s as SqlQueryParameter).Value
                    } : (SqlParameter)s;
                // newP.ParameterName = "@" + newP.ParameterName;
                return newP;
            }).ToArray();

            using (var dr = await Database.ExecuteSqlQueryAsync(sql, cancellationToken, ps))
            {
                // Output rows.
                var reader = dr.DbDataReader;
                read(reader);
            }
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().Result;
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            int userid = 0;
            if (principal != null)
            {
                userid = principal.GetUserId();
                if (userid == 0)
                    return await base.SaveChangesAsync(cancellationToken);
            }
            var models = ChangeTracker.Entries<AuditEntity>();
            var CreatedAt = DateTime.UtcNow;
            var CreatedAtLong = CreatedAt.ToUnixTimeMilliSeconds();
            foreach (var entity in models)
            {
                if (entity.State == EntityState.Added)
                {
                    if (entity.Entity.CreatedAt == DateTime.MinValue)
                        entity.Entity.CreatedAt = CreatedAt;
                    if (entity.Entity.CreatedById == 0)
                        entity.Entity.CreatedById = userid;
                }
                else if (entity.State == EntityState.Modified || entity.State == EntityState.Deleted
                    ) // || entity.State == EntityState.Unchanged
                {
                    //if (entity.Entity.UpdatedAt == null)
                        entity.Entity.UpdatedAt = CreatedAt;
                    //if (entity.Entity.UpdatedById == null) // we should override updater every time
                        entity.Entity.UpdatedById = userid;
                }
            }
            var trackingModels = ChangeTracker.Entries<TrackingAuditEnity>();
            foreach (var entity in trackingModels)
            {
                if (entity.State == EntityState.Added)
                {
                    //if (entity.Entity.CreatedById == 0)
                    //    entity.Entity.CreatedById = userid ?? 0;
                    if (entity.Entity.CreatedAt == 0)
                        entity.Entity.CreatedAt = CreatedAtLong;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging(true);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CommonDbContext).Assembly);

            modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole()
            {
                Id = 1,
                Name = "Administrator",
                IsBuiltIn = true,
                NormalizedName = "Administrator".ToUpper(),
                Title = "مدیر",
                ConcurrencyStamp = "F4A2BE58-8911-4689-A102-70B48937B298"
            });
            // new SeedDMSDB(modelBuilder).SeedAll();

            // modelBuilder.HasSequence<long>("DBSequenceHiLo").StartsAt(1000).IncrementsBy(10);
            // modelBuilder.ForSqlServerUseSequenceHiLo("DBSequenceHiLo"); 

            //modelBuilder.Entity<RecentRefineryUnitMonthData>().ToView("RecentRefineryUnitMonthDatas", "Planning").HasKey(s=>s.EntityId);
            //modelBuilder.Entity<RecentRefineryInputCommitmentData>().Ignore(s => s.Entity);


        }

        public IDbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }
    }
}
