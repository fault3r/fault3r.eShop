
using System;
using AccountService.Domain.ValueObjects;

namespace AccountService.Tests;

public class OtherTests
{
    [Fact]
    public void OtherTest()
    {
        var id1 = new Identity();
        var id2 = new Identity(id1.Id);
        Assert.True(id1 == id2);
    }
}