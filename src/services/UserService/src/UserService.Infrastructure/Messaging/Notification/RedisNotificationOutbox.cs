
using System;
using System.Text.Json;
using UserService.Application.Interfaces;
using UserService.Domain.Contracts;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging.Notification;
using StackExchange.Redis;
using UserService.Infrastructure.Exceptions.Security.Authentication;
using Polly;
using Polly.Retry;

namespace UserService.Infrastructure.Messaging.Notification;

public sealed class RedisNotificationOutbox : INotificationOutbox
{
    private readonly IDatabase _database;
    private readonly INotificationFactory _factory;

    private const string StreamKey = "stream-notification";
    private const string GroupName = "group-notification";
    private const string ConsumerName = "consumer-outbox";

    private readonly JsonSerializerOptions jsonOptions = SharedJsonOptions.DefaultOptions;

    private readonly AsyncRetryPolicy retryPolicy;

    public RedisNotificationOutbox(
        IDatabase database,
        INotificationFactory notificationFactory)
    {
        _database = database;
        _factory = notificationFactory;

        retryPolicy = Policy
            .Handle<RedisConnectionException>()
            .Or<RedisTimeoutException>()
            .Or<RedisServerException>()
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(200 * attempt)
            );

        var init = Initial();
        init.Wait();
    }

    public async Task Initial(CancellationToken cancellationToken = default)
    {
        try
        {
            await _database.StreamCreateConsumerGroupAsync(
                key: StreamKey,
                groupName: GroupName,
                position: StreamPosition.Beginning,
                createStream: true
            );
        }
        catch (RedisServerException ex) when (ex.Message.Contains("BUSYGROUP"))
        { }

        catch (Exception) { throw; }
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

        var committed = await retryPolicy.ExecuteAsync(
            async ct => await transaction.ExecuteAsync(),
            cancellationToken
        );

        if (!committed)
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
        await retryPolicy.ExecuteAsync(
            async ct => await _database.StreamAcknowledgeAsync(
                key: StreamKey,
                groupName: GroupName,
                messageId: message.StreamId
            ),
            cancellationToken
        );
    }
}
