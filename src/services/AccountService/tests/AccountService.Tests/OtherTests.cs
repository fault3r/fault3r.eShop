
using System;
using AccountService.Domain.Aggregates.Account;
using AccountService.Domain.Common;
using AccountService.Domain.Factories;
using AccountService.Domain.ValueObjects;
using FluentAssertions;

namespace AccountService.Tests;

public class OtherTests
{
    [Fact]
    public void OtherTest()
    {
        var uu = AccountFactory.CreateNew(
            "Hamed Damavandi", "hamed@email.com", "password");
        uu.ChangeFullName("fault3r");
        Assert.True(true);
    }
}