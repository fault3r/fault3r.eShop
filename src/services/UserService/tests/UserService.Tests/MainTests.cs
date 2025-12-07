
using System;
using UserService.Domain.Events.User;
using UserService.Domain.ValueObjects;

namespace UserService.Tests;

public class MainTests
{
    [Fact]
    public void TestName()
    {
        var
        @event
        =
        new
        UserCreatedEvent
        (
        Identity
        .
        New()
        ,
        Email
        .
        Parse
        (
        "ex@e"
        )
        )
        ;
    }
}
