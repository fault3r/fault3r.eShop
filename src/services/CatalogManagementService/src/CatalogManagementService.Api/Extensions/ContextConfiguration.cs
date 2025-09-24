
using System;
using CatalogManagementService.Infrastructure.Configurations;
using CatalogManagementService.Infrastructure.Data.Contexts;
using MongoDB.Driver;

namespace CatalogManagementService.Api.Extensions
{
    public static class ContextConfiguration
    {
        public static IServiceCollection AddContextConfiguration(this IServiceCollection services,
            ContextSettings settings)
        {
            services.AddSingleton<MongoClient>(provider => new MongoClient(settings.ConnectionString));
            services.AddScoped<CatalogContext>(provider =>
            {
                var client = provider.GetRequiredService<MongoClient>();
                return new CatalogContext(client, settings.DatabaseName, settings.CollectionName);
            });
            return services;
        }
    }
}