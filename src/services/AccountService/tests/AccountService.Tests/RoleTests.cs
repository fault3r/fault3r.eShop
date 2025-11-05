
using System;
using AccountService.Domain.Accounts;
using AccountService.Domain.Exceptions;

namespace AccountService.Tests;

public class RoleTests
{
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
    public void From_WithValidName_CreatesNewInstance(string input, string expected)
    {
        var role = Role.From(input);
        Assert.Equal(role.Name, expected);
    }

    [Fact]
    public void Equality_WorksCorrectly()
    {
        var roleUser = Role.User;
        var roleAdmin = Role.Admin;
        Assert.False(roleUser.Equals(null));
        Assert.False(roleUser.Equals(roleAdmin));
        Assert.True(roleUser.Equals(Role.User)); ;
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
        result = left != right;
        Assert.Equal(!result, expected);                            
    }

    [Fact]
    public void Casts_WithValidString_CreatesNewInstance()
    {
        string user = "User";
        string admin = "ADMIN";
        Assert.Equal(((Role)user).Name, user);
        Assert.Equal(((Role)admin).Name, Role.Admin);
    }
}