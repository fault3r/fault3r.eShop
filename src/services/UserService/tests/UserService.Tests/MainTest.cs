
using System;
using System.Text.Json;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.CrossCutting.JsonSerializer;

namespace UserService.Tests;

public class MainTests
{
    [Fact]
    public void Test()
    {
        var user = User.Create(
            Identity.From(Guid.NewGuid()),
            Email.Parse("test@example.com"),
            PasswordHash.Parse("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"),
            PasswordSalt.Parse("salt"),
            FullName.From("John", "Doe"),
            Role.User,
            Status.Pending
        );

        var options = new AppJsonSerializer().DefaultOptions;

        var @event = user.Events.FirstOrDefault();

        var serialized = JsonSerializer.Serialize(@event, @event.GetType(), options);

        var obj = JsonSerializer.Deserialize(serialized,  @event.GetType(), options);

        Assert.True(true);
    }
}