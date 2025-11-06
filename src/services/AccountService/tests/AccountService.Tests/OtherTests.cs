
using System;
using AccountService.Domain.Factories;

namespace AccountService.Tests;

public class OtherTests
{
    [Fact]
    public void TestName()
    {
        var acc = AccountFactory.Create("hamed damavandi", "hamed@email.com", "asdadhkjh535hk3534");
        Assert.True(true);
    }
}
