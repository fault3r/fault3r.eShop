
using System;
using AccountService.Domain.Common;
using AccountService.Domain.Factories;
using AccountService.Domain.ValueObjects;

namespace AccountService.Tests;

public class OtherTests
{
    [Fact]
    public void TestName()
    {
        var identity = Identity.From(Guid.Empty.ToString());
        Assert.True(true);
    }
}
