
using System;
using System.Text;
using System.Text.Json;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.Interfaces;
using CatalogManagementService.Infrastructure.Configurations;
using RabbitMQ.Client;

namespace CatalogManagementService.Infrastructure.EventBus
{
    public class RabbitmqEventPublisher : IEventPublisher
    {
        private readonly IConnection _connection;

        private readonly IModel channel;

        private readonly RabbitmqSettings settings;

        public RabbitmqEventPublisher(
            IConnection connection, RabbitmqSettings settings)
        {
            _connection = connection;
            channel = _connection.CreateModel();
            this.settings = settings;
            InitialChannel();
        }

        private void InitialChannel()
        {
            channel.ExchangeDeclare(
                exchange: settings.ExchangeName,
                type: ExchangeType.Direct,
                durable: true);
            channel.QueueDeclare(
                queue: settings.QueueName,
                durable: true,
                exclusive: false);
            channel.QueueBind(
                queue: settings.QueueName,
                exchange: settings.ExchangeName,
                routingKey: settings.RoutingKey);
        }

        private static JsonSerializerOptions JsonOptions =>
            new() { WriteIndented = true };
            
        public Task<bool> PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            //log
            Console.WriteLine($"***{nameof(RabbitmqEventPublisher)} is trying to publish an event.");
            try
            {
                var jsonBody = JsonSerializer.Serialize<TEvent>(@event, JsonOptions);
                var body = Encoding.UTF8.GetBytes(jsonBody);
                var properties = channel.CreateBasicProperties();
                properties.Type = typeof(TEvent).Name;
                channel.BasicPublish(
                    exchange: settings.ExchangeName,
                    routingKey: settings.RoutingKey,
                    basicProperties: properties,
                    body: body);
                return Task.FromResult(true);
            }
            catch { throw; }
        }
    }
}