
using System;
using UserService.Domain.Aggregates.User;
using UserService.Domain.ValueObjects;

namespace UserService.Tests;

public class MainTests
{
    [Fact]
    public void TestName()
    {
        var user = new User(Identity.New());
        var user2 = new User(Identity.Parse(user.Id.ToString()));
        Assert.True(user.Equals(user2));
    }
}
