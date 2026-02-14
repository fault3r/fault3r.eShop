
using System;
using UserService.Domain.Exceptions.Email;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.UnitTests.UserService.Domain.Tests.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Parse_WithValidEmail_ReturnsNormalizedEmail()
    {
        var email = Email.Parse("Sauron@Example.COM");

        Assert.Equal("Sauron@example.com", email.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Parse_WithMissingEmail_ThrowsMissingEmailException(string? input)
    {
        Assert.Throws<MissingEmailException>(() => Email.Parse(input!));
    }

    [Theory]
    [InlineData("missing-at.com")]
    [InlineData("@no-local-part.com")]
    [InlineData("local@")]
    public void Parse_WithInvalidEmail_ThrowsInvalidEmailException(string input)
    {
        Assert.Throws<InvalidEmailException>(() => Email.Parse(input));
    }
}
