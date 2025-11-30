using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Aggregates.User;
using UserService.Domain.Aggregates.User.Events;
using UserService.Domain.ValueObjects;
using UserService.Tests.ValueObjects;

namespace UserService.Tests;

public class MainTests
{
    [Fact]
    public void TestName()
    {
        var events = new UserCreatedDomainEvent(
            Identity.New(), Email.Parse("example@e.com"));

            var (id, email) = events;
    }
}
