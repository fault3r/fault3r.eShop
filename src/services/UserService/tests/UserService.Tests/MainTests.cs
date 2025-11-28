using System;
using UserService.Domain.ValueObjects;
using UserService.Tests.ValueObjects;

namespace UserService.Tests;

public class MainTests
{
    [Fact]
    public void TestName()
    {
        Identity? id = null;
        var idt = Identity.New();
        Assert.False(id.Equals(idt));
    }
}
