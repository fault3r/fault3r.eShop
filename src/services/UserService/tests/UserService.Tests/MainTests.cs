using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Aggregates.User;
using UserService.Domain.ValueObjects;
using UserService.Tests.ValueObjects;

namespace UserService.Tests;

public class MainTests
{
    [Fact]
    public void TestName()
    {
        var user = new User(Identity.New());
        var user2 = new User(Identity.New());
        var res = user.ToString();
        user.Equals(user2);
    }
}
