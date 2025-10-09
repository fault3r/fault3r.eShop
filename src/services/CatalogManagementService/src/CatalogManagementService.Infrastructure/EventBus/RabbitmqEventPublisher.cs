
using System;
using System.Text;
using System.Text.Json;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.Events.BaseEvent;
using CatalogManagementService.Infrastructure.Configurations;
using RabbitMQ.Client;

namespace CatalogManagementService.Infrastructure.EventBus
{
    public class RabbitmqEventPublisher : IEventPublisher
    {

        private readonly IConnection _connection;

        private readonly IModel channel;

        private readonly RabbitmqSettings settings;

        public RabbitmqEventPublisher(IConnection connection, RabbitmqSettings settings)
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
                durable: true,
                autoDelete: false,
                arguments: null);
            channel.QueueDeclare(
                queue: settings.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(
                queue: settings.QueueName,
                exchange: settings.ExchangeName,
                routingKey: settings.RoutingKey);
        }

        public bool Publish<T>(T @event)
            where T : ItemEvent
        {
            try
            {
                var json = JsonSerializer.Serialize<T>(@event);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(
                    exchange: settings.ExchangeName,
                    routingKey: settings.RoutingKey,
                    basicProperties: null,
                    body: body);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
    }
}