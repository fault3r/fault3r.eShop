
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
        var result = Assert.Throws<DomainException>(act);
        Assert.Equal("Role is required", result.Message);
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

    [Theory]
    [InlineData("User", "User", true)]
    [InlineData(null, null, true)]
    [InlineData(null, "User", false)]
    [InlineData("User", null, false)]
    [InlineData("User", "Admin", false)]
    public void OperatorEquals_WorksAsExpected(string? leftInput, string? rightInput, bool expected)
    {
        Role? left = leftInput is null ? null : Role.From(leftInput);
        Role? right = rightInput is null ? null : Role.From(rightInput);
        bool result = left == right;
        Assert.Equal(result, expected);
    }

    [Theory]
    [InlineData("Admin", "User", false)]
    [InlineData(null, "User", true)]
    [InlineData("User", null, false)]
    [InlineData("User", "Admin", true)]
    [InlineData("User", "User", false)]
    public void OperatorLessThan_WorksAsExpected(string? leftInput, string rightInput, bool expected)
    {
        Role? left = leftInput is null ? null : Role.From(leftInput);
        Role? right = rightInput is null ? null : Role.From(rightInput);
        Assert.False(left < right);
    }

    [Fact]
    public void ExplicitCast_WithValidString_CreatesNewInsttance()
    {
        string user = "User";
        string admin = "Admin";
        Role roleUser = (Role)user;
        Role roleAdmin = (Role)admin;
        Assert.Equal("User", roleUser.Name);
        Assert.Equal("Admin", roleAdmin.Name);
    }

    [Fact]
    public void ExplicitCast_WithInvalidString_ThrowsException()
    {
        string name = "user.";
        void act() => _ = (Role)name;
        var result = Assert.Throws<DomainException>(act);
        Assert.Equal($"Invalid role: {name}", result.Message);
    }

    [Fact]
    public void ImplicitCast_WithInvalidString_ThrowsException()
    {
        Role role = Role.User;
        string roleName = role;
        Assert.Equal("User", roleName);

    }
}