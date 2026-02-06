using System.Text;
using RabbitMQ.Client;
using UserService.Domain.Messaging.Outbox;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.Messaging.EventBus;

public sealed class RabbitmqEventPublisher(IModel channel, RabbitmqSettings settings)
{
    private readonly IModel _channel = channel;
    private readonly RabbitmqSettings _settings = settings;

    public Task PublishAsync(OutboxMessage message, CancellationToken cancellationToken = default)
    {
        var body = Encoding.UTF8.GetBytes(message.Payload);

        var props = _channel.CreateBasicProperties();
        props.MessageId = message.Id.ToString();
        props.CorrelationId = message.CorrelationId;
        props.ContentType = "application/json";
        props.DeliveryMode = 2; 

        _channel.BasicPublish(
            exchange: _settings.Exchange,
            routingKey: message.Type,
            basicProperties: props,
            body: body
        );

        return Task.CompletedTask;
    }
}
