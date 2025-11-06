
using System;
using System.Reflection.Metadata;
using AccountService.Domain.Common;

namespace AccountService.Tests;

public class OtherTests
{
    [Fact]
    public void TestName()
    {
        var id = Identity.New();
        string idstr = "";
        var id2 = Identity.From(idstr);
        Assert.Equal(id, id2);
    }
}
