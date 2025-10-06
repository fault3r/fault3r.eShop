using System;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Infrastructure.Configurations;
using CatalogManagementService.Infrastructure.EventBus;

namespace CatalogManagementService.Api.Configurations
{
    public static class RabbitmqConfiguration
    {
        public static IServiceCollection AddRabbitmqConfiguration(this IServiceCollection services,
            RabbitmqSettings settings)
        {
            services.AddScoped<IEventPublisher, RabbitmqEventPublisher>();
            return services;
        }
    }    
}