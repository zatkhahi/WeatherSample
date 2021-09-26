using HRM.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        private readonly IConfiguration configuration;

        public MyDbContextFactory()
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: false, reloadOnChange: true);

            this.configuration = builder.Build();
        }
        public MyDbContext CreateDbContext(string[] args)
        {
            var cs = configuration.GetConnectionString("MyDbConnection");
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseSqlServer(cs, o =>
            {
            });

            return new MyDbContext(optionsBuilder.Options);
        }
    }

}
