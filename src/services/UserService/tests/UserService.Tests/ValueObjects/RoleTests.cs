
using System;
using UserService.Domain.Exceptions.ValueObjects.Role;
using UserService.Domain.ValueObjects;
using static UserService.Domain.ValueObjects.Role;

namespace UserService.Tests.ValueObjects;

public class RoleTests
{
    [Fact]
    public void WithRoleType_SetValue()
    {
        var role = new Role(RoleType.User);

        Assert.Equal(RoleType.User, role.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void WithEmptyRoleNameString_ThrowMissingRoleNameException(string? input)
    {
        Assert.Throws<MissingRoleNameException>(() => new Role(input!));
    }

    [Fact]
    public void WithInvalidRoleNameString_ThrowUnsupportedRoleNameException()
    {
        Assert.Throws<UnsupportedRoleNameException>(() => new Role("not-a-role"));
    }

    [Theory]
    [InlineData("user", RoleType.User)]
    [InlineData("User", RoleType.User)]
    [InlineData(" USER ", RoleType.User)]
    [InlineData("admin", RoleType.Admin)]
    [InlineData("Admin", RoleType.Admin)]
    public void WithValidRoleNameString_SetValue(string input, RoleType expected)
    {
        var role = new Role(input);

        Assert.Equal(expected, role.Value);
    }
}
