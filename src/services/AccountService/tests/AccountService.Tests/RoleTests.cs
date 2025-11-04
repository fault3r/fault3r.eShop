
using System;
using AccountService.Domain.Accounts;
using AccountService.Domain.Exceptions;

namespace AccountService.Tests;

public class RoleTests
{
    [Fact]
    public void AdminAndUser_StaticFields_AreNotNullAndHaveExpectedNames()
    {
        var admin = Role.Admin;
        var user = Role.User;
        Assert.NotNull(admin);
        Assert.NotNull(user);
        Assert.Equal("Admin", admin.Name);
        Assert.Equal("User", user.Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void From_WithNullOrWhiteSpace_ThrowsDomainException(string? input)
    {
        void act() => Role.From(input!);
        var actual = Assert.Throws<DomainException>(act);
        Assert.Equal("Role is required", actual.Message);
    }

    [Theory]
    [InlineData("Admin", "Admin")]
    [InlineData("ADMIN", "Admin")]
    [InlineData("User", "User")]
    [InlineData("user", "User")]
    public void From_WithValidNames_CreatesNewInstance(string input, string expected)
    {
        var role = Role.From(input);
        Assert.Equal(role.ToString(), expected);
    }

    [Fact]
    public void ToString_WhenCalled_ReturnsRoleName()
    {
        var role = Role.User;
        string roleName = role.ToString();
        Assert.Equal("User", roleName);
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        var role = Role.User;
        bool result = role.Equals(null);
        Assert.False(result);
    }

    [Fact]
    public void Equals_WithDifferentNames_ReturnsFalse()
    {
        var role1 = Role.User;
        var role2 = Role.Admin;
        bool result = role1.Equals(role2);
        Assert.False(result);
    }

    [Fact]
    public void Equals_WithSameName_ReturnsTrue()
    {
        var role1 = Role.Admin;
        var role2 = Role.Admin;
        bool result = role1.Equals(role2);
        Assert.True(result);
    }
}