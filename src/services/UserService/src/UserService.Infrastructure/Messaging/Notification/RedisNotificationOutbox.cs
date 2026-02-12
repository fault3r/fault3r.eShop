
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

    private readonly JsonSerializerOptions jsonOptions
        = SharedJsonOptions.DefaultOptions;

    public async Task EnqueueAsync(
        IEnumerable<IDomainEvent> events,
        string correlationId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(events);
        ArgumentException.ThrowIfNullOrWhiteSpace(correlationId);

        if (!events.Any()) return;

        if (events.Any(e => e is null))
            throw new ArgumentException($"{nameof(events)} contains null element");

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
                CorrelationId = correlationId,
            };

            var payload = JsonSerializer.Serialize(message, jsonOptions);

            _ = transaction.SetAddAsync(SetKey, payload);
        }

        if (!await transaction.ExecuteAsync())
            throw new RedisTransactionFailedException();
    }

    public async Task<IEnumerable<NotificationMessage>> DequeueAsync(
        CancellationToken cancellationToken = default)
    {
        var values = await _database.SetMembersAsync(SetKey);

        if (values.Length == 0) return [];

        return values.Select(e => JsonSerializer.Deserialize<NotificationMessage>(e!, jsonOptions))!;
    }

    public async Task MarkAsProcessedAsync(
        Guid notificationId,
        CancellationToken cancellationToken = default)
    {

    }
    
    public string GetKey(Guid notificationId)
        => $"notification:{notificationId}";
}

