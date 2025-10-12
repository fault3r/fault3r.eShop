
using System;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Infrastructure.Configurations;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CatalogManagementService.Infrastructure.EventBus
{
    public class RabbitmqEventHandler
    {
        private readonly IConnection _connection;

        private readonly IServiceProvider _provider;

        private readonly IModel channel;

        private readonly string queueName;

        public RabbitmqEventHandler(
            IConnection connection, IServiceProvider provider, string queueName)
        {
            _connection = connection;
            channel = _connection.CreateModel();
            this.queueName = queueName;
            _provider = provider;
            InitialChannel();
        }

        private void InitialChannel()
        {
            channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false);
        }

        public void Subscribe<ItemEvent>()
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var strBody = Encoding.UTF8.GetString(body);
                var theEvent = JsonSerializer.Deserialize<ItemEvent>(strBody);

                using var scope = _provider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<ItemEvent>>();
                await handler.Handle(theEvent);      
            };
        }
    }
}