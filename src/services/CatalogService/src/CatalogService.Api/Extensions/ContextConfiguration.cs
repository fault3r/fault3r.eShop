
using System;
using CatalogService.Infrastructure.Configurations;
using CatalogService.Infrastructure.Data.Contexts;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CatalogService.Api.Extensions
{
    public static class ContextConfiguration
    {
        public static IServiceCollection AddContextConfiguration(this IServiceCollection services,
            IConfigurationSection configuration)
        {
            services.Configure<ContextSettings>(configuration);
            services.AddSingleton<MongoClient>(provider =>
            {
                var settings = provider.GetRequiredService<IOptions<ContextSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
                // MongoClient must be thread-safe.
            });
            services.AddScoped<CatalogContext>(provider =>
            {
                var settings = provider.GetRequiredService<IOptions<ContextSettings>>().Value;
                var client = provider.GetRequiredService<MongoClient>();
                return new CatalogContext(client, settings.DatabaseName, settings.CollectionName);
            });
            return services;
        }
    }
}