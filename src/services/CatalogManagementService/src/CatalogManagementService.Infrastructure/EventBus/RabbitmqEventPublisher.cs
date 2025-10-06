
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

        public async Task PublishAsync<T>(T @event)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();
            await channel.ExchangeDeclareAsync(
                exchange: "exhchange",
                type: ExchangeType.Direct
            );
            var body = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(@event));
            await channel.BasicPublishAsync(
                exchange: "exchange",
                routingKey: "queue",
                body: body
            );
        }
    }
}