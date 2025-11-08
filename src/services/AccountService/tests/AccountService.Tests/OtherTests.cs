
using System;
using AccountService.Domain.Common;
using AccountService.Domain.Factories;
using AccountService.Domain.ValueObjects;
using static AccountService.Domain.ValueObjects.Status;

namespace AccountService.Tests;

public class OtherTests
{
    [Fact]
    public void TestName()
    {
        var roleA = new Role("aDmIn");
        var roleB = Role.User;
        Assert.True(roleA != roleB);

    }
}