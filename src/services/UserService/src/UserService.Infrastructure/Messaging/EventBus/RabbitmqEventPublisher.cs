
using System;
using System.Text;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using UserService.Domain.Messaging.Outbox;

namespace UserService.Infrastructure.Messaging.EventBus;

public sealed class RabbitmqEventPublisher
{
    private readonly IModel _channel;
    private readonly string exchangeName;
    private readonly AsyncRetryPolicy retryPolicy;

    public RabbitmqEventPublisher(IModel channel,
        string exchangeName, string exchangeType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(exchangeName);
        ArgumentException.ThrowIfNullOrWhiteSpace(exchangeType);

        _channel = channel;
        this.exchangeName = exchangeName;

        channel.ExchangeDeclare(
            exchange: exchangeName,
            type: exchangeType,
            durable: true,
            autoDelete: false,
            arguments: null
        );
        channel.ConfirmSelect();

        retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: (retryAttempt) =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            );
    }

    public async Task PublishAsync(
        OutboxMessage message,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        var props = _channel.CreateBasicProperties();
        props.MessageId = message.Id.ToString();
        props.CorrelationId = message.CorrelationId;
        props.DeliveryMode = 2;
        props.ContentType = "application/json";

        var body = Encoding.UTF8.GetBytes(message.Payload);

        await retryPolicy.ExecuteAsync(async (ct) =>
        {
            _channel.BasicPublish(
                exchange: exchangeName,
                routingKey: message.Type,
                basicProperties: props,
                body: body
            );

            _channel.WaitForConfirmsOrDie();
        }, cancellationToken);
    }
}
