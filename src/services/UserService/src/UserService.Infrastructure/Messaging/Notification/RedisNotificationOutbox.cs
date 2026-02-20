
using System;
using System.Text.Json;
using UserService.Application.Interfaces;
using UserService.Domain.Contracts;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging.Notification;
using StackExchange.Redis;
using Polly;
using Polly.Wrap;
using Polly.Timeout;

namespace UserService.Infrastructure.Messaging.Notification;

public sealed class RedisNotificationOutbox(
    IDatabase database,
    INotificationFactory notificationFactory
) : INotificationOutbox
{
    private readonly IDatabase _database = database;
    private readonly INotificationFactory _factory = notificationFactory;

    private AsyncPolicyWrap policy = default!;

    private const string StreamKey = "stream-notification";
    private const string GroupName = "group-notification";
    private const string ConsumerName = "consumer-outbox";

    private readonly JsonSerializerOptions jsonOptions = SharedJsonOptions.DefaultOptions;

    public async Task Initialize()
    {
        var timeoutPolicy = Policy
            .TimeoutAsync(
                timeout: TimeSpan.FromSeconds(10),
                timeoutStrategy: TimeoutStrategy.Optimistic,
                onTimeoutAsync: async (_, delay, _, _) =>
                {
                    Console.WriteLine($"{this} Redis operation timed out after {delay.TotalSeconds} seconds!");
                    await Task.CompletedTask;
                }
        );

        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 2,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(attempt * 2),
                onRetryAsync: async (_, delay, attempt, _) =>
                {
                    Console.WriteLine($"{this}: Redis failed! Retry {attempt} after {delay.TotalSeconds} seconds!");
                    await Task.CompletedTask;
                }
            );

        policy = retryPolicy.WrapAsync(timeoutPolicy);

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
        {
            Console.WriteLine($"{this}: redis consumer group already exists.");
        }
    }

    public async Task EnqueueAsync(
        IDomainEvent @event,
        string correlationId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentException.ThrowIfNullOrWhiteSpace(correlationId);

        var notification = _factory.FromEvent(@event, correlationId);

        var entries = new NameValueEntry[]
        {
            new("Id" , @event.EventId.ToString()),
            new("Type", notification.GetType().Name),
            new("Payload", JsonSerializer.Serialize(notification, notification.GetType(), jsonOptions)),
            new("Timestamp", @event.OccurredOn.ToString("O")),
            new("CorrelationId", correlationId),
        };

        await policy.ExecuteAsync(
            async ct =>
            {
                await _database.StreamAddAsync(StreamKey, entries);
            },
            cancellationToken
        );
    }

    public async Task<NotificationMessage?> DequeueAsync(
        CancellationToken cancellationToken = default)
    {
        var entries = await policy.ExecuteAsync(
            async ct =>
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

                return entries;
            }, cancellationToken);

        if (entries.Length == 0) return null;

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
        await policy.ExecuteAsync(
            async ct =>
            {
                await _database.StreamAcknowledgeAsync(
                    key: StreamKey,
                    groupName: GroupName,
                    messageId: message.StreamId
                );
            },
            cancellationToken
        );
    }
}
