

using System;
using CatalogManagementService.Infrastructure.Configurations;
using CatalogManagementService.Infrastructure.Data.Contexts;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CatalogManagementService.Api.Extensions
{
    public static class ContextConfiguration
    {
        public static IServiceCollection AddContextConfiguration(this IServiceCollection services,
            IConfigurationSection configuration)
        {
            services.Configure<ContextSettings>(configuration);
            services.AddSingleton<MongoClient>((provider) =>
            {
                var settings = provider.GetRequiredService<IOptions<ContextSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
                
            });
            services.AddScoped<CatalogManagementContext>((provider) =>
            {
                var settings = provider.GetRequiredService<IOptions<ContextSettings>>().Value;
                var client = provider.GetRequiredService<MongoClient>();
                return new CatalogManagementContext(client, settings.DatabaseName, settings.CollectionName);
            });
            return services;
        }
    }

}