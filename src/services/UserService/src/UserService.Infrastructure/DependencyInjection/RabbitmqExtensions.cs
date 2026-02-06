
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using UserService.Infrastructure.Exceptions.DependencyInjection;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.DependencyInjection;

public static class RabbitmqExtensions
{
    public static IServiceCollection AddRabbitmqMessageBroker(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(nameof(RabbitmqSettings)).Get<RabbitmqSettings>()
            ?? throw new MissingRabbitmqSettingsException();

        services.AddSingleton(settings);

        services.AddSingleton<IConnectionFactory>(_ =>
            new ConnectionFactory
            {
                HostName = settings.HostName,
                Port = settings.Port,
                UserName = settings.UserName,
                Password = settings.Password,
            });

        services.AddSingleton<IConnection>(sp =>
        {
            var factory = sp.GetRequiredService<IConnectionFactory>();
            return factory.CreateConnection();
        });

        services.AddSingleton<IModel>(sp =>
        {
            var connection = sp.GetRequiredService<IConnection>();
            var channel = connection.CreateModel();

            var s = sp.GetRequiredService<RabbitmqSettings>();
            channel.ExchangeDeclare(
                exchange: s.Exchange,
                type: s.ExchangeType,
                durable: true,
                autoDelete: false,
                arguments: null
            );

            return channel;
        });

        return services;
    }
}
