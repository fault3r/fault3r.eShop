
using System;
using CatalogService.Application.Interfaces;
using CatalogService.Infrastructure.Configurations;
using CatalogService.Infrastructure.Data.Contexts;
using MongoDB.Driver;

namespace CatalogService.Api.Configurations
{
    public static class MongoContextConfiguration
    {
        public static IServiceCollection AddMongoContextConfiguration(this IServiceCollection services,
            ConfigurationManager configuration)
        {
            var _logger = services.BuildServiceProvider()
                .GetRequiredService<ILoggerService<Program>>();
            try
            {
                _logger.LogInformation("configuring MongoContext..");
                var settings = configuration.GetSection(nameof(MongoSettings))
                    .Get<MongoSettings>() ??
                    throw new Exception();
                services.AddSingleton<MongoClient>(provider =>
                {
                    return new MongoClient(settings.ConnectionString);
                });
                services.AddScoped<MongoContext>(provider =>
                {
                    var client = provider.GetRequiredService<MongoClient>();
                    var logger = provider.GetRequiredService<ILoggerService<MongoContext>>();
                    return new MongoContext(client, settings.DatabaseName, settings.CollectionName, logger);
                });
                _logger.LogInformation("MongoContext configured successfully.");
                return services;
            }
            catch
            {
                _logger.LogError("failed to configure MongoContext settings!");
                throw new InvalidOperationException(nameof(Program));
            }
        }
    }
}