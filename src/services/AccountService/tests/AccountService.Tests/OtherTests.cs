
using System;
using System.Reflection.Metadata;
using AccountService.Domain.Common;

namespace AccountService.Tests;

public class OtherTests
{
    [Fact]
    public void TestName()
    {
        var res = Result.Ok();
        Assert.True(res.IsSuccess);

        res = Result.Fail("failed");
        Assert.False(res.IsSuccess);
        Assert.Equal("failed", res.Message);       

    }
}
