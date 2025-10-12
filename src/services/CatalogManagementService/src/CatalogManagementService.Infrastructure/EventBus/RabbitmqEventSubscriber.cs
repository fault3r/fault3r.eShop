
using System;
using System.Text;
using System.Text.Json;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.Interfaces;
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
            IConnection connection, IServiceProvider provider, RabbitmqSettings settings)
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

        public void Subscribe<TEvent>() where TEvent : IEvent
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var strBody = Encoding.UTF8.GetString(body);
                var @event = JsonSerializer.Deserialize<TEvent>(strBody);

                using var scope = _provider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<TEvent>>();
                await handler.Handle(@event!);
            };
            channel.BasicConsume(
                queue: settings.QueueName,
                autoAck: true,
                consumer: consumer);
        }

    }
}