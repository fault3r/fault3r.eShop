
using System;
using System.Text;
using System.Text.Json;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace CatalogManagementService.Infrastructure.EventBus
{
    public class RabbitmqEventPublisher(IOptions<RabbitmqSettings> settings) : IEventPublisher
    {
        private readonly RabbitmqSettings _settings = settings.Value;

        public async Task PublishAsync<T>(T @event)
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
            };
            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();
            await channel.ExchangeDeclareAsync(
                exchange: _settings.QueueName,
                type: ExchangeType.Direct
            );
            var body = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(@event));
            await channel.BasicPublishAsync(
                exchange: _settings.QueueName,
                routingKey: _settings.QueueName,
                body: body
            );
        }
    }
}