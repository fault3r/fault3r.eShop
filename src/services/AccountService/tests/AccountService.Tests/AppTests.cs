using AccountService.Domain.ValueObjects;

namespace AccountService.Tests;

public class AppTests
{
    [Fact]
    public void Test1()
    {
        var role1 = Role.Admin();
        var role2 = Role.Admin();
        Assert.True(role1 == role2);
    }
}
