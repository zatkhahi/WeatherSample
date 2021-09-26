using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Security.Principal;
using WeatherSample.Application.Interfaces;
using WeatherSample.Domain;

namespace HRM.Persistence
{
    public class MyDbContext : CommonDbContext, IWeatherDbContext
    {

        public MyDbContext(DbContextOptions options, IPrincipal principal = null)
           : base(options, principal)
        {
            this.principal = principal;
        }

        public DbSet<CityWeatherData> CityWeatherDatas { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlServer(options.Options);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyDbContext).Assembly);

            seedRoles(modelBuilder);
            seedUsers(modelBuilder);
        }

        void seedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole()
            {
                Id = 2,
                Name = "WeatherUser",
                IsBuiltIn = true,
                NormalizedName = "WeatherUser".ToUpper(),
                Title = "Weather Api User",
                ConcurrencyStamp = "D4091538-7C24-422A-968B-667D3FE32039"
            });
        }

        void seedUsers(ModelBuilder modelBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = 1,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "m.zatkhahi@gmail.com",
                EmailConfirmed = true,
                FirstName = "Mohammad",
                LastName = "Zatkhahi",
                ConcurrencyStamp = "99e999c6-0e08-4d9b-bd45-f5decf3c1563",
                NormalizedEmail = "m.zatkhahi@gmail.com".ToUpper(),
                PasswordHash = hasher.HashPassword(null, "123456")
            });

            modelBuilder.Entity<ApplicationUserRole>().HasData(
                new ApplicationUserRole
                {
                    RoleId = 1,
                    UserId = 1
                });
        }

    }
}
