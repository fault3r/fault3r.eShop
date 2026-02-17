
using System;
using StackExchange.Redis;
using UserService.Application.Messaging.Notification;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.Messaging.Notification;

namespace UserService.Tests;

public class MainTests
{
    [Fact]
    public async void Test()
    {
        var connection = ConnectionMultiplexer.Connect("localhost:6379");

        var database = connection.GetDatabase();

        var factory = new NotificationFactory();

        var outbox = new RedisNotificationOutbox(database, factory);

        await outbox.EnqueueAsync(
            events: [new UserRegisteredEvent(
                userId: Identity.From(Guid.NewGuid()),
                email: Email.Parse("hamed@ex.com"),
                fullName: FullName.Parse("Hamed Damaavandi")
            )],
            correlationId: "00000000000",
            cancellationToken: default
        );
    }
}