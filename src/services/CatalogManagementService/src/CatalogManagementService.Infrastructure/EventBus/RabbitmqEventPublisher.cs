
using System;
using System.Text;
using System.Text.Json;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Infrastructure.Configurations;
using RabbitMQ.Client;

namespace CatalogManagementService.Infrastructure.EventBus
{
    public class RabbitmqEventPublisher : IEventPublisher
    {

        private readonly IConnection _connection;

        private readonly IModel channel;

        private readonly RabbitmqSettings _settings;

        public RabbitmqEventPublisher(IConnection connection, RabbitmqSettings settings)
        {
            _settings = settings;
            _connection = connection;
            channel = _connection.CreateModel();
            InitialChannel();
        }

        public void InitialChannel()
        {
            channel.ExchangeDeclare(
                exchange: _settings.ExchangeName,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                arguments: null);
            channel.QueueDeclare(
                queue: _settings.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(
                queue: _settings.QueueName,
                exchange: _settings.ExchangeName,
                routingKey: _settings.RoutingKey);
        }

        public (bool Success, string Message) PublishAsync<TEvent>(TEvent @event)
            where TEvent : class
        {
            var json = JsonSerializer.Serialize<TEvent>(@event);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(
                exchange: _settings.ExchangeName,
                routingKey: _settings.RoutingKey,
                basicProperties: null,
                body: body);
            return (true, $"{nameof(TEvent)} published.");
        }
    }
}