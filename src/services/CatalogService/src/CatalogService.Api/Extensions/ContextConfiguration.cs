
using System;
using CatalogService.Infrastructure.Configurations;
using CatalogService.Infrastructure.Data.Contexts;
using Microsoft.Extensions.Options;

namespace CatalogService.Api.Extensions
{
    public static class ContextConfiguration
    {
        public static IServiceCollection AddContextConfiguration(this IServiceCollection services,
            IConfigurationSection configuration)
        {
            services.Configure<ContextSettings>(configuration);
            services.AddSingleton<CatalogContext>(provider =>
            {
                var settings = provider.GetRequiredService<IOptions<ContextSettings>>().Value;
                return new CatalogContext(settings);
            });
            return services;
        }
    }
}