
using System;
using System.Text.Json;
using UserService.Application.Interfaces;
using UserService.Domain.Contracts;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging.Notification;
using StackExchange.Redis;

namespace UserService.Infrastructure.Messaging.Notification;

public sealed class RedisNotificationOutbox(
    IConnectionMultiplexer redisConnection,
    INotificationFactory factory
) : INotificationOutbox
{
    private readonly IDatabase _database = redisConnection.GetDatabase();
    private readonly INotificationFactory _factory = factory;

    private readonly JsonSerializerOptions jsonOptions
        = SharedJsonOptions.DefaultOptions;
        
    private const string queue = "notification";

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

        var notifications = events
            .Select(e => OutboxMessage.FromEvent(e, correlationId));

        await _dbContext.OutboxMessages
            .AddRangeAsync(messages, cancellationToken);

        var notification = _factory.FromEvent(@event, correlationId);

        var message = new NotificationMessage
        {
            Id = @event.EventId,
            EnqueuedOn = @event.OccurredOn,
            Type = notification.GetType().Name,
            Payload = JsonSerializer.Serialize(
                notification, notification.GetType(), jsonOptions),
        };

        var payload = JsonSerializer.Serialize(message, jsonOptions);

        await _database.ListLeftPushAsync(queue, payload);
    }

    public async Task<NotificationMessage?> DequeueAsync(CancellationToken cancellationToken = default)
    {
        var payload = await _database.ListRightPopAsync(queue);

        if (payload.IsNullOrEmpty) return null;

        return JsonSerializer.Deserialize<NotificationMessage>(payload!, jsonOptions);
    }
}

