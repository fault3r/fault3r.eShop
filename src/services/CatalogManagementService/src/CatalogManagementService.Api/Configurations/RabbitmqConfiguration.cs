using System;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Infrastructure.Configurations;
using CatalogManagementService.Infrastructure.EventBus;

namespace CatalogManagementService.Api.Configurations
{
    public static class RabbitmqConfiguration
    {
        public static IServiceCollection AddRabbitmqConfiguration(this IServiceCollection services,
            ConfigurationManager configuration)
        {
            var settingsSection = configuration.GetSection(nameof(RabbitmqSettings)) ??
                throw new NullReferenceException();

            services.Configure<RabbitmqSettings>(settingsSection);
            services.AddScoped<IEventPublisher, RabbitmqEventPublisher>();
            return services;
        }
    }    
}