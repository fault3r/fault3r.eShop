
using System;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Infrastructure.Configurations;
using CatalogManagementService.Infrastructure.Data.Contexts;
using MongoDB.Driver;

namespace CatalogManagementService.Api.Configurations
{
    public static class MongoContextConfiguration
    {
        public static IServiceCollection AddMongoContextConfiguration(this IServiceCollection services,
            ConfigurationManager configuration)
        {
            var _logger = services.BuildServiceProvider()
                .GetRequiredService<ILoggerService<Program>>();
            _logger.LogInformation("configuring MongoContext..");
            var settings = configuration.GetSection(nameof(MongoSettings))
                .Get<MongoSettings>() ??
                throw new NullReferenceException(nameof(MongoSettings));
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
    }
}