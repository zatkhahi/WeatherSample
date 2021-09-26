using Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;
using System.Security.Principal;
using WeatherSample.Application.Interfaces;

namespace HRM
{
    public class Startup : Common.Presentation.Startup
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        {
        }


        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            ConfigureMediaRService(services, typeof(IWeatherDbContext).Assembly);

            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    //builder.WithOrigins("http://localhost:4200")
                    //.AllowAnyHeader().AllowAnyMethod().Build();
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build();
                });
            });

            services.AddDbContext<Persistence.MyDbContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("MyDbConnection"),
                        sqlServerOptions =>
                        {
                            sqlServerOptions.CommandTimeout(30);
                        }), ServiceLifetime.Scoped);
            services.AddScoped<ICommonDbContext>(sp => sp.GetRequiredService<Persistence.MyDbContext>());
            services.AddScoped<IWeatherDbContext>(sp => sp.GetRequiredService<Persistence.MyDbContext>());
           
            ConfigureJWTService<Persistence.MyDbContext>(services);


            services.AddScoped(provider => provider.GetService<IHttpContextAccessor>()?.HttpContext?.User ?? Application.Authorization.CommonAuthorizeHelper.GetSystemPrincipal());
             

            var mvcBuilder = services.AddMvc(op => op.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    //options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

                }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddSingleton(s =>
            {
                var r = new Newtonsoft.Json.JsonSerializer()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                };
                
                return r;
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });


            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp";
            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("EnableCORS");

            //if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization(); // must be between UseRouting and UseEndpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseMvc();
            //app.MapWhen(context =>
            //{
            //    var path = context.Request.Path.Value;
            //    return path.StartsWith("/downloads/", StringComparison.OrdinalIgnoreCase);
            //}, c => c.UseStaticFiles());

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                    // spa.UseAngularCliServer(npmScript: "start");
                }

            });
            //app.UseRouting();
        }

    }
}
