
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

        private readonly IServiceProvider _provider;

        private readonly IModel channel;

        private readonly RabbitmqSettings settings;

        public RabbitmqEventSubscriber(
            IConnection connection, RabbitmqSettings settings, IServiceProvider provider)
        {
            _connection = connection;
            channel = _connection.CreateModel();
            this.settings = settings;
            _provider = provider;
            InitialChannel();
        }

        private void InitialChannel()
        {
            channel.QueueDeclare(
                queue: settings.QueueName,
                durable: true,
                exclusive: false);
        }

        public Task Subscribe()
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (_, ea) =>
            {
                string eventName = ea.BasicProperties.Type;
                var eventType = eventName switch
                {
                    nameof(ItemCreatedEvent) => typeof(ItemCreatedEvent),
                    nameof(ItemUpdatedEvent) => typeof(ItemUpdatedEvent),
                    nameof(ItemDeletedEvent) => typeof(ItemDeletedEvent),
                    _ => throw new InvalidOperationException()
                };
                var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
                using var scope = _provider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService(handlerType);
                var handlerMethod = handlerType.GetMethod("Handle");
                var body = ea.Body.ToArray();
                var strBody = Encoding.UTF8.GetString(body);
                var @event = JsonSerializer.Deserialize(strBody, eventType);
                handlerMethod?.Invoke(handler, [@event]);
            };
            channel.BasicConsume(
                queue: settings.QueueName,
                autoAck: true,
                consumer: consumer);
            return Task.CompletedTask;
        }
    }
}