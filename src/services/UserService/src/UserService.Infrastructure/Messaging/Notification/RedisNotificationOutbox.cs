
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
    INotificationMapper mapper
) : INotificationOutbox
{
    private readonly IDatabase _database = redisConnection.GetDatabase();
    private readonly INotificationMapper _mapper = mapper;

    private readonly JsonSerializerOptions jsonOptions
        = SharedJsonOptions.DefaultOptions;
        
    private const string queue = "notification";

    public async Task EnqueueAsync(
        IDomainEvent @event,
        string correlationId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(@event);

        var notification = _mapper.FromEvent(@event, correlationId);

        var message = new NotificationMessage
        {
            Id = @event.EventId,
            EnqueuedOn = @event.OccurredOn,
            Type = notification.GetType().Name,
            Payload = JsonSerializer.Serialize(
                notification, notification.GetType(), jsonOptions),
            CorrelationId = correlationId,
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

