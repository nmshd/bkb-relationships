using System;
using System.Text.Json.Serialization;
using Enmeshed.BuildingBlocks.API.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Relationships.API.Extensions;
using Relationships.API.JsonConverters;
using Relationships.Application;
using Relationships.Application.Extensions;
using Relationships.Infrastructure.EventBus;
using Relationships.Infrastructure.Persistence;

namespace Relationships.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApplicationOptions>(_configuration.GetSection("ApplicationOptions"));

            services.AddCustomAspNetCore(_configuration, _env, options =>
            {
                options.Authentication.Audience = "relationships";
                options.Authentication.Authority = _configuration.GetAuthorizationConfiguration().Authority;
                options.Authentication.ValidIssuer = _configuration.GetAuthorizationConfiguration().ValidIssuer;

                options.Cors.AllowedOrigins = _configuration.GetCorsConfiguration().AllowedOrigins;
                options.Cors.ExposedHeaders = _configuration.GetCorsConfiguration().ExposedHeaders;

                options.HealthChecks.SqlConnectionString = _configuration.GetSqlDatabaseConfiguration().ConnectionString;

                options.Json.Converters.Add(new RelationshipTemplateIdJsonConverter());
                options.Json.Converters.Add(new RelationshipIdJsonConverter());
                options.Json.Converters.Add(new RelationshipChangeIdJsonConverter());
                options.Json.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddCustomApplicationInsights();

            services.AddCustomFluentValidation(options => { });

            services.AddPersistence(options =>
            {
                options.DbOptions.DbConnectionString = _configuration.GetSqlDatabaseConfiguration().ConnectionString;

                options.BlobStorageOptions.ConnectionString = _configuration.GetBlobStorageConfiguration().ConnectionString;
                options.BlobStorageOptions.ContainerName = "relationships";
            });

            services.AddEventBus(_configuration.GetEventBusConfiguration());

            services.AddApplication();

            return services.ToAutofacServiceProvider();
        }

        public void Configure(IApplicationBuilder app, TelemetryConfiguration telemetryConfiguration)
        {
            telemetryConfiguration.DisableTelemetry = !_configuration.GetApplicationInsightsConfiguration().Enabled;

            app.ConfigureMiddleware(_env);
        }
    }
}
