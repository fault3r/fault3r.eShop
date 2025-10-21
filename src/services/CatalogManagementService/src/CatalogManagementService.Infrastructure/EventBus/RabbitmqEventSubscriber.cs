
using System;
using System.Text;
using System.Text.Json;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.Events;
using CatalogManagementService.Infrastructure.Configurations;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CatalogManagementService.Infrastructure.EventBus
{
    public class RabbitmqEventSubscriber
    {
        private readonly IConnection _connection;

        private readonly IModel channel;

        private readonly RabbitmqSettings settings;

        private readonly IServiceProvider _provider;

        private readonly ILoggerService<RabbitmqEventSubscriber> _logger;

        public RabbitmqEventSubscriber(
            IConnection connection, RabbitmqSettings settings, IServiceProvider provider,
            ILoggerService<RabbitmqEventSubscriber> logger)
        {
            _connection = connection;
            channel = _connection.CreateModel();
            this.settings = settings;
            _provider = provider;
            InitialChannel();
            _logger = logger;
            _logger.LogInformation("instance created.");
        }

        private void InitialChannel()
        {
            channel.QueueDeclare(
                queue: settings.QueueName,
                durable: true,
                exclusive: false);
        }

        public Task StartSubscribeAsync()
        {
            try
            {
                _logger.LogInformation("creating consumer..");
                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.Received += async (_, ea) =>
                {
                    await _logger.LogInformation($"received {ea.BasicProperties.Type}.");
                    string eventName = ea.BasicProperties.Type;
                    var eventType = eventName switch
                    {
                        nameof(ItemCreatedEvent) => typeof(ItemCreatedEvent),
                        nameof(ItemUpdatedEvent) => typeof(ItemUpdatedEvent),
                        nameof(ItemDeletedEvent) => typeof(ItemDeletedEvent),
                        _ => throw new Exception()
                    };
                    var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
                    string methodName = "HandleAsync";
                    var handlerMethod = handlerType.GetMethod(methodName);
                    using var scope = _provider.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService(handlerType);
                    var body = ea.Body.ToArray();
                    var strBody = Encoding.UTF8.GetString(body);
                    var @event = JsonSerializer.Deserialize(strBody, eventType);
                    handlerMethod?.Invoke(handler, [@event]);
                    await _logger.LogInformation($"handled {ea.BasicProperties.Type}.");
                };
                channel.BasicConsume(
                    queue: settings.QueueName,
                    autoAck: true,
                    consumer: consumer);
                _logger.LogInformation("consumer created successfully.");
                return Task.CompletedTask;
            }
            catch
            {
                _logger.LogError("an unexpected error has occurred.");
                throw new InvalidOperationException(nameof(RabbitmqEventSubscriber));
            }
        }
    }
}