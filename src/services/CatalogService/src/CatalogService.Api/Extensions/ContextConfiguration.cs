
using System;
using CatalogService.Infrastructure.Configurations;
using CatalogService.Infrastructure.Data.Contexts;
using Microsoft.Extensions.Options;

namespace CatalogService.Api.Extensions
{
    public static class ContextConfiguration
    {
        public static IServiceCollection AddContextConfiguration(this IServiceCollection services, IConfigurationSection settings)
        {
            services.Configure<MongodbSettings>(settings);
            services.AddSingleton<CatalogContext>(provider =>
            {
                var settings = provider.GetRequiredService<IOptions<MongodbSettings>>().Value;
                return new CatalogContext(settings);
            });
            return services;
        }
    }
}