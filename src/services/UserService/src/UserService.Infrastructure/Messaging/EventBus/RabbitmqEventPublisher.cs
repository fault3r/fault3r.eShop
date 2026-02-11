
using System;
using System.Text;
using RabbitMQ.Client;
using UserService.Domain.Messaging.Outbox;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.Messaging.EventBus;

public sealed class RabbitmqEventPublisher
{
    private readonly IModel _channel;
    private readonly RabbitmqSettings _settings;

    public RabbitmqEventPublisher(IModel channel,
    RabbitmqSettings settings)
    {
        _channel = channel;
        _settings = settings;

        channel.ExchangeDeclare(
            exchange: settings.ExchangeName,
            type: settings.ExchangeType,
            durable: true,
            autoDelete: false,
            arguments: null
        );
    }

    public Task PublishAsync(
        OutboxMessage message,
        CancellationToken cancellationToken = default)
    {
        var body = Encoding.UTF8.GetBytes(message.Payload);

        var props = _channel.CreateBasicProperties();
        props.MessageId = message.Id.ToString();
        props.CorrelationId = message.CorrelationId;
        props.ContentType = "application/json";
        props.DeliveryMode = 2;

        _channel.BasicPublish(
            exchange: _settings.ExchangeName,
            routingKey: message.Type,
            basicProperties: props,
            body: body
        );

        return Task.CompletedTask;
    }
}
