
using System;
using UserService.Domain.Exceptions.Email;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.UnitTests.Domain.Tests.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Parse_WithValidEmail_ReturnsNormalizedEmail()
    {
        var email = Email.Parse("Email@Example.Com");

        Assert.Equal("Email@example.com", email.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    [InlineData("")]
    public void Parse_WithMissingEmail_ThrowsMissingEmailException(string? input)
    {
        Email act() => Email.Parse(input!);

        Assert.Throws<MissingEmailException>(act);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@no-local-part")]
    [InlineData("no-domain-part@")]
    public void Parse_WithInvalidEmail_ThrowsInvalidEmailException(string input)
    {
        Email act() => Email.Parse(input);

        Assert.Throws<InvalidEmailException>(act);
    }
}
