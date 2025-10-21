
using System;
using System.Net.Sockets;
using CatalogManagementService.Api.HostedServices;
using CatalogManagementService.Application.EventHandlers;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.Events;
using CatalogManagementService.Infrastructure.Configurations;
using CatalogManagementService.Infrastructure.EventBus;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace CatalogManagementService.Api.Configurations
{
    public static class RabbitmqConfiguration
    {
        public static IServiceCollection AddRabbitmqConfiguration(this IServiceCollection services,
            ConfigurationManager configuration)
        {
            var _logger = services.BuildServiceProvider()
                .GetRequiredService<ILoggerService<Program>>();
            try
            {
                _logger.LogInformation("configuring RabbitMQ..");
                var settings = configuration.GetSection(nameof(RabbitmqSettings))
                    .Get<RabbitmqSettings>() ??
                    throw new NullReferenceException(nameof(RabbitmqSettings));
                services.AddSingleton<IConnection>(provider =>
                {
                    try
                    {
                        _logger.LogInformation("creating RabbitMQ connection..");
                        var factory = new ConnectionFactory
                        {
                            HostName = settings.HostName,
                            UserName = settings.UserName,
                            Password = settings.Password,
                            DispatchConsumersAsync = true,
                        };
                        IConnection? connection = null;
                        var retryPolicy = Policy
                            .Handle<BrokerUnreachableException>()
                            .Or<SocketException>()
                            .Or<TimeoutException>()
                            .WaitAndRetry(
                                retryCount: 3,
                                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                                onRetry: (exception, timeSpan, attempt, context) =>
                                {
                                    Console.WriteLine($"attempting to create RabbitMQ connection #{attempt}");
                                    _logger.LogError($"attempting to create RabbitMQ connection #{attempt}");
                                });
                        retryPolicy.Execute(() =>
                        {
                            connection = factory.CreateConnection();
                        });
                        _logger.LogInformation("RabbitMQ connection created successfully.");
                        return connection ?? throw new Exception();
                    }
                    catch
                    {
                        _logger.LogError("failed to create RabbitMQ connection!");
                        throw new InvalidOperationException(nameof(RabbitmqSettings));
                    }
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
                    var logger = provider.GetRequiredService<ILoggerService<RabbitmqEventSubscriber>>();
                    return new RabbitmqEventSubscriber(connection, settings, provider, logger);
                });
                services.AddHostedService<RabbitmqEventSubscriberHostedService>();
                _logger.LogInformation("RabbitMQ configured successfully.");
                return services;
            }
            catch
            {
                _logger.LogError("failed to configure RabbitMQ settings!");
                throw new InvalidOperationException(nameof(Program));
            }
        }
    }
}