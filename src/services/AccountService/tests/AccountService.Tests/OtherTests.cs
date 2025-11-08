
using System;
using AccountService.Domain.Common;
using AccountService.Domain.Factories;
using AccountService.Domain.ValueObjects;
using static AccountService.Domain.ValueObjects.Status;

namespace AccountService.Tests;

public class OtherTests
{
    [Fact]
    public void OtherTest()
    {
        var sts = new Status(StatusType.Pending);
        var sts1 = Status.From("active");
        var sts3 = new Status("pending");
        Assert.True(sts == sts3);
        Assert.True(sts1.Value == StatusType.Active);
    }
}