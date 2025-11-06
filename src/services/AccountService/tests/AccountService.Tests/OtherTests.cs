
using System;
using System.Reflection.Metadata;
using AccountService.Domain.Base;
using AccountService.Domain.Common;

namespace AccountService.Tests;

public class OtherTests
{
    [Fact]
    public void TestName()
    {
        var agg = new AggregateRoot();
        string jj = agg.Id.ToString();
        Assert.True(true);
    }
}
