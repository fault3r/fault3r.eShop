
using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
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
            channel.ExchangeDeclare(
                exchange: _settings.QueueName,
                ExchangeType.Direct);
        }

        public (bool Success, string Message) PublishAsync<TEvent>(TEvent @event) where TEvent : class
        {

            var json = JsonSerializer.Serialize<TEvent>(@event);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(
                exchange: _settings.QueueName,
                routingKey: _settings.QueueName,
                basicProperties: null,
                body: body
            );
            return (true, "Message published.");
        }
    }
}