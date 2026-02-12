
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Messaging.EventBus;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.DependencyInjection;

public static class RabbitmqExtensions
{
    public static IServiceCollection AddRabbitmqMessageBroker(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(nameof(RabbitmqSettings))
            .Get<RabbitmqSettings>()
                ?? throw new MissingRabbitmqSettingsException();

        services.AddSingleton<IConnection>(_ =>
        {
            var factory = new ConnectionFactory
            {
                HostName = settings.HostName,
                Port = settings.Port,
                UserName = settings.UserName,
                Password = settings.Password,

                DispatchConsumersAsync = true,
            };

            return factory.CreateConnection();
        });

        services.AddSingleton<IModel>(sp =>
        {
            var connection = sp.GetRequiredService<IConnection>();
            return connection.CreateModel();
        });

        services.AddSingleton<RabbitmqEventPublisher>(sp =>
        {
            var channel = sp.GetRequiredService<IModel>();
            return new(channel, settings.ExchangeName, settings.ExchangeType);
        });

        return services;
    }
}
