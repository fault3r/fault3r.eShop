
using System;
using AccountService.Domain.Common;

namespace AccountService.Tests.UnitTests.Domain.Common;

public class IdentityTests
{

    [Fact]
    public void TestName()
    {
        var identity1 = new Identity();
        var identity2 = new Identity(Guid.NewGuid());
        var identity3 = Identity.New();
        var identity4 = Identity.From(identity1.ToString());
        Assert.True(true);
    }
}
