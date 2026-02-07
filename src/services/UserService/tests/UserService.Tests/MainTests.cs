
using System;
using System.Security.Cryptography;
using System.Text;
using UserService.Domain.Contracts;

namespace UserService.Tests;

public class MainTests
{
    [Fact]
    public void TestName()
    {
        var res = RandomStringGenerator.Generate();

        Console.WriteLine(res);

        Assert.True(true);
    }
}
