
using System;
using System.Text.Json;
using UserService.Application.Interfaces;
using UserService.Domain.Contracts;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging.Notification;
using StackExchange.Redis;
using UserService.Infrastructure.Exceptions.Security.Authentication;

namespace UserService.Infrastructure.Messaging.Notification;

public sealed class RedisNotificationOutbox(
    IDatabase database,
    INotificationFactory factory
) : INotificationOutbox
{
    private readonly IDatabase _database = database;
    private readonly INotificationFactory _factory = factory;
    private const string Key = "notification";

    private readonly JsonSerializerOptions jsonOptions
        = SharedJsonOptions.DefaultOptions;

    public async Task EnqueueAsync(
        IEnumerable<IDomainEvent> events,
        string correlationId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(events);
        ArgumentException.ThrowIfNullOrWhiteSpace(correlationId);

        var transaction = _database.CreateTransaction();

        foreach (var @event in events)
        {
            var notification = _factory.FromEvent(@event, correlationId);

            var message = new NotificationMessage
            {
                Id = @event.EventId,
                Type = notification.GetType().Name,
                Payload = JsonSerializer.Serialize(
                    notification, notification.GetType(), jsonOptions),
                Timestamp = @event.OccurredOn,
                CorrelationId = correlationId
            };

            var payload = JsonSerializer.Serialize(message, jsonOptions);

            _ = transaction.ListLeftPushAsync(Key, payload);
        }

        if (!await transaction.ExecuteAsync())
            throw new RedisTransactionFailedException();
    }

    public async Task<NotificationMessage?> DequeueAsync(
        CancellationToken cancellationToken = default)
    {
        var value = await _database.ListRightPopAsync(Key);

        if (value.IsNullOrEmpty) return null;

        return JsonSerializer.Deserialize<NotificationMessage>(value!, jsonOptions);
    }

    public async Task RequeueAsync(
        NotificationMessage message,
        CancellationToken cancellationToken = default)
    {
        if (message is null) return;

        var payload = JsonSerializer.Serialize(message, jsonOptions);

        await _database.ListRightPushAsync(Key, payload);
    }
}
