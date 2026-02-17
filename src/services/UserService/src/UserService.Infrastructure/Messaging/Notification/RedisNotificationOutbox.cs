
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

    private const string StreamKey = "stream-notification";
    private const string GroupName = "group-notification";
    private const string ConsumerName = "consumer-outbox";

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
            throw new Exception("Failed to initialize Redis consumer group");
    }

    public async Task<bool> InitialConsumer(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _database.StreamCreateConsumerGroupAsync(
                key: StreamKey,
                groupName: GroupName,
                position: StreamPosition.Beginning,
                createStream: true
            );

            return result;
        }
        catch (RedisServerException ex) when (ex.Message.Contains("BUSYGROUP"))
        {
            return true; // group already exists
        }
        catch
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
                new("Timestamp", @event.OccurredOn.ToString("O")),
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
        var pending = await _database.StreamReadGroupAsync(
            key: StreamKey,
            groupName: GroupName,
            consumerName: ConsumerName,
            position: StreamPosition.Beginning,
            count: 1
        );

        var entries = pending.Length > 0
            ? pending
            : await _database.StreamReadGroupAsync(
                key: StreamKey,
                groupName: GroupName,
                consumerName: ConsumerName,
                position: StreamPosition.NewMessages,
                count: 1
            );

        if (entries.Length == 0)
            return null;

        var entry = entries[0];
        var dict = entry.Values.ToDictionary(e => e.Name, e => e.Value);

        return new NotificationMessage
        {
            Id = Guid.Parse(dict["Id"]!),
            Type = dict["Type"]!,
            Payload = dict["Payload"]!,
            Timestamp = DateTimeOffset.Parse(dict["Timestamp"]!),
            StreamId = entry.Id!,
            CorrelationId = dict["CorrelationId"]!,
        };
    }

    public async Task MarkAsProcessedAsync(
        NotificationMessage message,
        CancellationToken cancellationToken = default)
    {
        await _database.StreamAcknowledgeAsync(
            key: StreamKey,
            groupName: GroupName,
            messageId: message.StreamId
        );
    }
}
