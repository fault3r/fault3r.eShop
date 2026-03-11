
using System;
using System.Text.Json;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Domain.Contracts;
using UserService.Domain.Factories;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.Messaging.Outbox;

namespace UserService.Tests;

public class MainTests
{

    [Fact]
    public void MainTest()
    {
        var email = Email.Parse("example@email.com");
        var hash = PasswordHash.Parse("$argon2id$v=19$m=65536,t=3,p=4$tq1euaOqS1ZcbrcLRLFb5w==$FULw8GEzOhG3YO1n54CSXk4pRl4yfALFRquP1Tn2UGE=");
        var salt = PasswordSalt.Parse(RandomStringGenerator.GetString(4));
        var name = FullName.From("Hamed", "Damaavandi");

        var user = UserFactory.Create(email, hash, salt, name);

        var @event = user.Events.First();

        var jsonOptions = SharedJsonSerializer.DefaultOptions;

        var type = OutboxTypeResolver.Resolve(@event.GetType().Name)!;

        var json = JsonSerializer.Serialize(@event, type, jsonOptions);

        var obj = JsonSerializer.Deserialize(json, type, jsonOptions);

        Assert.NotNull(json);
        Assert.NotNull(obj);
    }
}