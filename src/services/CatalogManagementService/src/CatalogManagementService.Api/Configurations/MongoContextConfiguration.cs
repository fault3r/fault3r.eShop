
using System;
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
            //log
            Console.WriteLine($"***{nameof(MongoContextConfiguration)} is being configured.");
            var settings = configuration.GetSection(nameof(MongoSettings))
                .Get<MongoSettings>() ??
                throw new NullReferenceException();
            services.AddSingleton<MongoClient>(provider =>
            {
                return new MongoClient(settings.ConnectionString);
            });
            services.AddScoped<MongoContext>(provider =>
            {
                var client = provider.GetRequiredService<MongoClient>();
                return new MongoContext(client, settings.DatabaseName, settings.CollectionName);
            });
            return services;
        }
    }
}