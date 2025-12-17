
using System;
using UserService.Infrastructure.CrossCutting;

namespace UserService.Tests;

public class MainTests
{


    [Fact]
    public void TestName()
    {
        CorrelationContext id = new();
        var corrId = id.CorrelationId;
    }
}
