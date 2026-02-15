
using System;
using System.Text.Json;
using UserService.Application.Interfaces;
using UserService.Domain.Contracts;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging.Notification;
using StackExchange.Redis;
using UserService.Infrastructure.Exceptions.Security.Authentication;

namespace UserService.Infrastructure.Messaging.Notification;

public sealed class RedisNotificationOutbox : INotificationOutbox
{
    private readonly IDatabase _database;
    private readonly INotificationFactory _factory;
    private const string StreamKey = "notification-stream";
    private const string GroupName = "group-application";
    private const string ConsumerName = "consumer-application";

    private readonly JsonSerializerOptions jsonOptions
        = SharedJsonOptions.DefaultOptions;

    public RedisNotificationOutbox(
        IDatabase database,
        INotificationFactory notificationFactory)
    {
        _database = database;
        _factory = notificationFactory;

        var init = InitialConsumer();
        init.Wait();

        if (!init.Result)
            throw new Exception();
    }

    public async Task<bool> InitialConsumer(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _database
                .StreamCreateConsumerGroupAsync(StreamKey, GroupName, StreamPosition.NewMessages);

            return result;
        }
        catch (RedisServerException ex) when (ex.Message.Contains("BUSYGROUP"))
        {
            return true;
        }
        catch(Exception)
        {
            return false;
        }
    }

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

            var entries = new NameValueEntry[]
            {
                new("Id" , @event.EventId.ToString()),
                new("Type", notification.GetType().Name),
                new("Payload", JsonSerializer.Serialize(notification, notification.GetType(), jsonOptions)),
                new("Timestamp", @event.OccurredOn.ToString()),
                new("CorrelationId", correlationId),
            };

            _ = transaction.StreamAddAsync(StreamKey, entries);
        }

        if (!await transaction.ExecuteAsync())
            throw new RedisTransactionFailedException();
    }

    public async Task<NotificationMessage?> DequeueAsync(
        CancellationToken cancellationToken = default)
    {
        var entries = await _database.StreamReadGroupAsync(StreamKey, GroupName, ConsumerName, StreamPosition.NewMessages, 1);

        if (entries.Length == 0) return null;

        var entry = entries[0];

       // turn entry to NotificationMessage and return it
    }

    public async Task MarkAsFailureAsync(
        NotificationMessage message,
        CancellationToken cancellationToken = default)
    {
        if (message is null) return;

        var payload = JsonSerializer.Serialize(message, jsonOptions);

        await _database.ListRightPushAsync(StreamKey, payload);
    }

    public async Task MarkAsProcessedAsync(
        NotificationMessage message,
        CancellationToken cancellationToken = default)
    {
        if (message is null) return;

        var payload = JsonSerializer.Serialize(message, jsonOptions);

        await _database.SetAddAsync($"{StreamKey}:processed", payload);
    }
}
