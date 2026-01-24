
using System;
using System.Text.Json;
using StackExchange.Redis;
using UserService.Application.Interfaces;
using UserService.Domain.Messaging.Notification;

namespace UserService.Infrastructure.Messaging.Notification;

public sealed class RedisNotificationOutbox(
    IConnectionMultiplexer redisConnection,
    IEventNotificationMapper mapper
) : INotificationOutbox
{
    private readonly IDatabase _database = redisConnection.GetDatabase();
    private readonly IEventNotificationMapper _mapper = mapper;

    public async Task EnqueueAsync(
        NotificationMessage notification,
        string correlationId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(notification);

        // var message = new NotificationMessage
        // {
        //     Id = @event.EventId,
        //     EnqueuedOn = @event.OccurredOn,
        //     Type = notification.GetType().Name,
        //     Payload = JsonSerializer.Serialize(
        //         notification, notification.GetType(), jsonSerializerOptions),
        //     CorrelationId = correlationId,
        // };

        var key = $"notification:{message.Id}";
        var payload = JsonSerializer.Serialize(message, jsonSerializerOptions);

        await _database.ListLeftPushAsync(key, payload);

    }

    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}

