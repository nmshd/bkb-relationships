﻿using System;
using Azure.Identity;
using Enmeshed.BuildingBlocks.API.Extensions;
using Enmeshed.Tooling.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Relationships.API.Extensions;
using Relationships.Infrastructure.Persistence.Database;

namespace Relationships.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build()
                .MigrateDbContext<ApplicationDbContext>((_, _) => { });

            host.Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                    options.AddServerHeader = false;
                    options.Limits.MaxRequestBodySize = 2.Mebibytes();
                })
                .ConfigureAppConfiguration(AddAzureAppConfiguration)
                .UseStartup<Startup>();
        }

        private static void AddAzureAppConfiguration(WebHostBuilderContext hostingContext, IConfigurationBuilder builder)
        {
            var configuration = builder.Build();

            var azureAppConfigurationConfiguration = configuration.GetAzureAppConfigurationConfiguration();

            if (azureAppConfigurationConfiguration.Enabled)
                builder.AddAzureAppConfiguration(appConfigurationOptions =>
                {
                    var credentials = new ManagedIdentityCredential();

                    appConfigurationOptions
                        .Connect(new Uri(azureAppConfigurationConfiguration.Endpoint), credentials)
                        .ConfigureKeyVault(vaultOptions => { vaultOptions.SetCredential(credentials); })
                        .Select("*", "")
                        .Select("*", "Relationships");
                });
        }
    }
}
