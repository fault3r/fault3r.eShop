
using System;
using CatalogManagementService.Api.HostedServices;
using CatalogManagementService.Application.EventHandlers;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.Events;
using CatalogManagementService.Infrastructure.Configurations;
using CatalogManagementService.Infrastructure.EventBus;
using RabbitMQ.Client;

namespace CatalogManagementService.Api.Configurations
{
    public static class RabbitmqConfiguration
    {
        public static IServiceCollection AddRabbitmqConfiguration(this IServiceCollection services,
            ConfigurationManager configuration)
        {
            var _logger = services.BuildServiceProvider()
                .GetRequiredService<ILoggerService<Program>>();
            _logger.LogInformation("configuring RabbitMQ..");
            var settings = configuration.GetSection(nameof(RabbitmqSettings))
                .Get<RabbitmqSettings>() ??
                throw new NullReferenceException(nameof(RabbitmqSettings));
            services.AddSingleton<IConnection>(provider =>
            {
                _logger.LogInformation("creating RabbitMQ connection..");
                var factory = new ConnectionFactory
                {
                    HostName = settings.HostName,
                    UserName = settings.UserName,
                    Password = settings.Password,
                };
                var connection = factory.CreateConnection();
                _logger.LogInformation("RabbitMQ connection created successfully.");
                return connection;
            });
            services.AddScoped<IEventPublisher>(provider =>
            {
                var connection = provider.GetRequiredService<IConnection>();
                var logger = provider.GetRequiredService<ILoggerService<RabbitmqEventPublisher>>();
                return new RabbitmqEventPublisher(connection, settings, logger);
            });
            services.AddScoped<IEventHandler<ItemCreatedEvent>, ItemCreatedEventHandler>();
            services.AddScoped<IEventHandler<ItemUpdatedEvent>, ItemUpdatedEventHandler>();
            services.AddScoped<IEventHandler<ItemDeletedEvent>, ItemDeletedEventHandler>();
            services.AddSingleton<RabbitmqEventSubscriber>(provider =>
            {
                var connection = provider.GetRequiredService<IConnection>();
                return new RabbitmqEventSubscriber(connection, settings, provider);
            });
            services.AddHostedService<RabbitmqEventSubscriberHostedService>();
            _logger.LogInformation("RabbitMQ configured successfully.");
            return services;
        }
    }
}