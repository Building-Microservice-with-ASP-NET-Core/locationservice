using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StatlerWaldorfCorp.LocationService.Models;
using StatlerWaldorfCorp.LocationService.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.Extensions.Hosting;

namespace StatlerWaldorfCorp.LocationService
{
    public class Startup
    {
        private ILogger logger;
        private ILoggerFactory loggerFactory;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            loggerFactory = LoggerFactory.Create(builder =>
             {
                 builder.AddConsole().AddDebug();
             });

            logger = loggerFactory.CreateLogger<Startup>();
        }
        public static IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            //var transient = Boolean.Parse(Configuration.GetSection("transient").Value);
            var transient = true;
            if (Configuration.GetSection("transient") != null)
            {
                transient = Boolean.Parse(Configuration.GetSection("transient").Value);
            }
            if (transient)
            {
                logger.LogInformation("Using transient location record repository.");
                services.AddScoped<ILocationRecordRepository, InMemoryLocationRecordRepository>();
            }
            else
            {
                var connectionString = Configuration.GetSection("postgres:cstr").Value;
                services.AddEntityFrameworkNpgsql().AddDbContext<LocationDbContext>(options =>
                    options.UseNpgsql(connectionString));
                logger.LogInformation("Using '{0}' for DB connection string.", connectionString);
                services.AddScoped<ILocationRecordRepository, LocationRecordRepository>();
            }

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllers();
            });
        }

        public static string[] Args { get; set; } = new string[] { };
        internal static IConfigurationRoot InitializeConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(Startup.Args);
            return builder.Build();
        }
    }
}
