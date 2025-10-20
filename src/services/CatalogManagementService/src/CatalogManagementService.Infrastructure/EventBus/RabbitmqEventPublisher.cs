
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

        private readonly ILoggerService<RabbitmqEventPublisher> _logger;

        public RabbitmqEventPublisher(
            IConnection connection, RabbitmqSettings settings,
            ILoggerService<RabbitmqEventPublisher> logger)
        {
            _connection = connection;
            channel = _connection.CreateModel();
            this.settings = settings;
            InitialChannel();
            _logger = logger;
            _logger.LogInformation("instance created.");
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
            try
            {
                _logger.LogInformation($"publishing {typeof(TEvent).Name} for id '{@event.Id}'..");
                var jsonBody = JsonSerializer.Serialize<TEvent>(@event, JsonOptions);
                var body = Encoding.UTF8.GetBytes(jsonBody);
                var properties = channel.CreateBasicProperties();
                properties.Type = typeof(TEvent).Name;
                channel.BasicPublish(
                    exchange: settings.ExchangeName,
                    routingKey: settings.RoutingKey,
                    basicProperties: properties,
                    body: body);
                _logger.LogInformation($"{typeof(TEvent).Name} published for id '{@event.Id}'.");
                return Task.FromResult(true);
            }
            catch
            {
                _logger.LogError("an unexpected error has occurred.");
                throw new InvalidOperationException(nameof(RabbitmqEventPublisher));
            }
        }
    }
}