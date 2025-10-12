using System;
using CatalogManagementService.Api.Services;
using CatalogManagementService.Application.EventHandlers;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.Events;
using CatalogManagementService.Infrastructure.Configurations;
using CatalogManagementService.Infrastructure.EventBus;
using RabbitMQ.Client;

namespace CatalogManagementService.Api.Configurations
{
    public static class RabbitmqHandlerConfiguration
    {
        public static IServiceCollection AddRabbitmqHandlerConfiguration(this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services.AddScoped<IEventHandler<ItemCreatedEvent>, ItemCreatedEventHandler>();
            var settings = configuration.GetSection(nameof(RabbitmqSettings))
                .Get<RabbitmqSettings>() ??
                throw new NullReferenceException();
            services.AddSingleton<RabbitmqEventSubscriber>(provider =>
            {
                var connection = provider.GetRequiredService<IConnection>();
                return new RabbitmqEventSubscriber(connection, provider, settings);
            });
            services.AddHostedService<RabbitmqEventSubscriberHostedService>();
            return services;
        }
    }
}

