
using System;
using System.Text.Json;
using UserService.Domain.Factories;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.CrossCutting;
using UserService.Infrastructure.CrossCutting.JsonSerializer;
using UserService.Infrastructure.Messaging.Outbox;

namespace UserService.Tests;

public class MainTests
{
    [Fact]
    public void TestName()
    {
        var id = Identity.From(Guid.NewGuid());

        var serializer = new NetJsonSerializer();

        var d = JsonSerializer.Serialize(id, serializer.Options);

        Assert.True(true);
    }

    [Fact]
    public void MainTest()
    {
        var email = Email.Parse("example@email.com");
        var hash = PasswordHash.Parse("$argon2id$v=19$m=65536,t=3,p=4$tq1euaOqS1ZcbrcLRLFb5w==$FULw8GEzOhG3YO1n54CSXk4pRl4yfALFRquP1Tn2UGE=");
        var salt = PasswordSalt.Parse(RandomStringGenerator.GetString(4));
        var name = FullName.From("Hamed", "Damaavandi");

        var user = UserFactory.Create(email, hash, salt, name);

        var @event = user.Events.First();

        var options = new NetJsonSerializer().Options;

        var type = EventTypeResolver.Resolve(@event.GetType().Name)!;

        var json = JsonSerializer.Serialize(@event, type, options);

        var obj = JsonSerializer.Deserialize(json, type, options);

        Assert.NotNull(json);
        Assert.NotNull(obj);
    }
}
