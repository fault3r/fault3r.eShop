
using System;
using UserService.Domain.Exceptions.PasswordHash;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.UnitTests.Domain.Tests.ValueObjects;

public class PasswordHashTests
{
    [Fact]
    public void Parse_WithValidPasswordHash_ReturnsPasswordHash()
    {
        string expected = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
        var hash = PasswordHash.Parse(expected);

        Assert.Equal(expected, hash.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    [InlineData("")]
    public void Parse_WithMissingPasswordHash_ThrowsMissingPasswordHashException(string? input)
    {
        PasswordHash act() => PasswordHash.Parse(input!);

        Assert.Throws<MissingPasswordHashException>(act);
    }

    [Fact]
    public void Parse_WithInvalidPasswordHash_ThrowsInvalidPasswordHashException()
    {
        PasswordHash act() => PasswordHash.Parse("short-length-exception");

        Assert.Throws<InvalidPasswordHashException>(act);
    }
}