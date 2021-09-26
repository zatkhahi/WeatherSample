using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Application.Interfaces;
using Common.Presentation.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Primitives;
using System.Net;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Application.Infrastructure;
using Application;

namespace Common.Presentation
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            //ConfigureAutoMapper(services);
            ConfigureSwaggerService(services);
            ConfigureMailServices(services);

            services.AddSingleton<IBackgourndTaskScheduler, BackgroundTaskScheduler>();
            services.AddHostedService<BackgroundTaskRunner>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddHostedService<QueuedHostedService>();
        }

        

        public void ConfigureMailServices(IServiceCollection services)
        {
            var mailSet = Configuration.GetSection("MailSettings");
            if (mailSet != null)
                services.Configure<MailSettings>(mailSet);

            services.AddTransient<IMailService, Services.MailKitService>();
        }

        //public void ConfigureAutoMapper(IServiceCollection services)
        //{
        //    var mappingConfig = new MapperConfiguration(mc =>
        //    {
        //        mc.AddProfile(new AutoMapperProfile());
        //    });

        //    IMapper mapper = mappingConfig.CreateMapper();
        //    services.AddSingleton(mapper);
        //}
        public void ConfigureMediaRService(IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddMediatR(typeof(ICommonDbContext).GetTypeInfo().Assembly);
            if (assemblies?.Length > 0)
                services.AddMediatR(assemblies);
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestAuthorizationBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        }

        public void ConfigureJWTService<TContext>(IServiceCollection services) where TContext : DbContext
        {

            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

            // within this section we are configuring the authentication and setting the default scheme
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // this will validate the 3rd part of the jwt token using the secret that we added in the appsettings and verify we have generated the jwt token
                    IssuerSigningKey = new SymmetricSecurityKey(key), // Add the secret key to our Jwt encryption
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
                jwt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = (context) => {
                        StringValues values;

                        if (!context.Request.Query.TryGetValue("access_token", out values))
                        {
                            return Task.CompletedTask;
                        }

                        if (values.Count > 1)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Fail(
                                "Only one 'access_token' query string parameter can be defined. " +
                                $"However, {values.Count:N0} were included in the request."
                            );

                            return Task.CompletedTask;
                        }

                        var token = values.Single();

                        if (String.IsNullOrWhiteSpace(token))
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Fail(
                                "The 'access_token' query string parameter was defined, " +
                                "but a value to represent the token was not included."
                            );

                            return Task.CompletedTask;
                        }

                        context.Token = token;

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<ApplicationRole>()
                            .AddEntityFrameworkStores<TContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
                options.User.RequireUniqueEmail = false;
            });
        }

        public void ConfigureSwaggerService(IServiceCollection services)
        {
            services.AddOpenApiDocument(s =>
            {
                s.DocumentName = "All APIs";
                //s.AddOperationFilter(filter =>
                //{
                //    if (filter.ControllerType.Name == "TokenController")
                //        return false;
                //    return true;
                //});

                s.AddSecurity("Authorization", new string[] { "Bearer" }, new NSwag.OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                    Type = NSwag.OpenApiSecuritySchemeType.ApiKey
                });

                s.PostProcess = d =>
                {
                    if (d.Consumes != null && d.Consumes.Count() > 0)
                    {
                        var items = new List<string>() { "application/json" };
                        items.AddRange(d.Consumes);
                        d.Consumes = items;
                    }
                    else
                        d.Consumes = new List<string>() { "application/json" };
                };
                // s.PostProcess = doc => doc.Produces = new List<string> { "application/json", "text/json" };
            });
        }
    }
}
