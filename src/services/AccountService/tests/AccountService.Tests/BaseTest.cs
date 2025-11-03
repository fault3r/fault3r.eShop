using AccountService.Domain.Accounts;

namespace AccountService.Tests;

public class BaseTest
{
    [Theory]
    [InlineData("Admin","Admin")]
    [InlineData("User","User")]
    public void From_WithValidNames_CreatesNewInstance(string input, string expected)
    {
        var adminRole = Role.From(input);
        
    }
}