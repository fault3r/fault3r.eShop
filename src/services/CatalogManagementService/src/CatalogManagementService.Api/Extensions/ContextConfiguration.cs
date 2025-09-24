
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
            ContextSettings settings)
        {
            services.AddSingleton<MongoClient>((provider) =>
            {
                return new MongoClient(settings.ConnectionString);
                
            });
            services.AddScoped<CatalogManagementContext>(provider =>
            {
                var client = provider.GetRequiredService<MongoClient>();
                return new CatalogManagementContext(client, settings.DatabaseName, settings.CollectionName);
            });
            return services;
        }
    }

}