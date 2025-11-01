using AccountService.Domain.ValueObjects;

namespace AccountService.Tests;

public class AppTests
{
    [Fact]
    public void Test1()
    {
        var role1 = Role.Admin();
        var role2 = Role.Admin();
        string test = role1.ToString();
        Assert.True(Role.Equals(role1, role2));
        Assert.Equal("Admin", test);
    }
}
