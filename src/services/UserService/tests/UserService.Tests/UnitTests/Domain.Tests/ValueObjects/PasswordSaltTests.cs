
using System;
using UserService.Domain.Exceptions.PasswordSalt;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.UnitTests.Domain.Tests.ValueObjects;

public class PasswordSaltTests
{
    [Fact]
    public void Parse_WithValidPasswordSalt_ReturnsPasswordSalt()
    {
        var salt = PasswordSalt.Parse("salt");

        Assert.Equal("salt", salt.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    [InlineData("")]
    public void Parse_WithMissingPasswordSalt_ThrowsMissingPasswordSaltException(string? input)
    {
        PasswordSalt act() => PasswordSalt.Parse(input!);

        Assert.Throws<MissingPasswordSaltException>(act);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("invalid-character")]
    public void Parse_WithInvalidPasswordSalt_ThrowsInvalidPasswordSaltException(string input)
    {
        PasswordSalt act() => PasswordSalt.Parse(input);

        Assert.Throws<InvalidPasswordSaltException>(act);
    }
}
