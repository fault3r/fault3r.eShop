using System;
using CatalogManagementService.Application.Interfaces;
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
            var settings = configuration.GetSection(nameof(RabbitmqSettings))
                .Get<RabbitmqSettings>() ??
                throw new NullReferenceException();
            services.AddSingleton<IConnection>(provider =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = settings.HostName,
                    UserName = settings.UserName,
                    Password = settings.Password,
                };
                return factory.CreateConnection();
            });
            services.AddScoped<IEventPublisher>(provider =>
            {
                var connection = provider.GetRequiredService<IConnection>();
                return new RabbitmqEventPublisher(connection, settings);
            });
            return services;
        }
    }
}