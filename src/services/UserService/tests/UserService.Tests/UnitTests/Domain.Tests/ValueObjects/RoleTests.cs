
using System;
using UserService.Domain.Exceptions.Role;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.UnitTests.Domain.Tests.ValueObjects;

public class RoleTests
{
    [Theory]
    [InlineData("User")]
    [InlineData("ADMIN")]
    [InlineData("unsupported")]
    public void Parse_WithValidRole_ReturnsRole(string input)
    {
        var role = Role.Parse(input);

        Assert.Equal(input.ToLower(), role.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    [InlineData("")]
    public void Parse_WithMissingRole_ThrowsMissingRoleException(string? input)
    {
        Role act() => Role.Parse(input!);

        Assert.Throws<MissingRoleException>(act);
    }

    [Fact]
    public void Parse_WithInvalidRole_ThrowsUnsupportedRoleException()
    {
        Role act() => Role.Parse("invalid");

        Assert.Throws<UnsupportedRoleException>(act);
    }
}
