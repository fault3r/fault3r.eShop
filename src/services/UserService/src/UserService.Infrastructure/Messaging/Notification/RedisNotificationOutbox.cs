
using System;
using System.Text.Json;
using StackExchange.Redis;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging.Notification;

namespace UserService.Infrastructure.Messaging.Notification;

public sealed class RedisNotificationOutbox(
    IConnectionMultiplexer redisConnection,
    INotificationMapper mapper
) : INotificationOutbox
{
    private readonly IDatabase _database = redisConnection.GetDatabase();
    private readonly INotificationMapper _mapper = mapper;

    private const string queue = "notification";

    public async Task EnqueueAsync(
        IDomainEvent @event,
        string correlationId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(@event);

        var notification = _mapper.FromEvent(@event);

        var message = new NotificationMessage
        {
            Id = @event.EventId,
            EnqueuedOn = @event.OccurredOn,
            Type = notification.GetType().Name,
            Payload = JsonSerializer.Serialize(
                notification, notification.GetType(), jsonSerializerOptions),
            CorrelationId = correlationId,
        };

        var payload = JsonSerializer.Serialize(message, jsonSerializerOptions);

        await _database.ListLeftPushAsync(queue, payload);
    }

    public async Task<NotificationMessage?> DequeueAsync(CancellationToken cancellationToken = default)
    {
        var payload = await _database.ListRightPopAsync(queue);

        if (payload.IsNullOrEmpty) return null;

        return JsonSerializer.Deserialize<NotificationMessage>(payload!, jsonSerializerOptions);
    }

    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}

