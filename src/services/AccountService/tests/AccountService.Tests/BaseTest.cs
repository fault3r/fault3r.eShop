
using System;
using AccountService.Domain.Accounts;
using AccountService.Domain.Exceptions;
using FluentAssertions;

namespace AccountService.Tests;

public class BaseTest
{
    [Theory]
    [InlineData("Admin", "Admin")]
    [InlineData("ADMIN", "Admin")]
    [InlineData("User", "User")]
    [InlineData("USER", "User")]
    public void From_WithValidNames_CreatesNewInstance(string input, string expected)
    {
        var role = Role.From(input);
        Assert.Equal(role.ToString(), expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void From_WithNullOrWhiteSpace_ThrowsDomainException(string? input)
    {
        var expected = new DomainException("Role is required");
        void act() => Role.From(input!);
        var actual = Assert.Throws<DomainException>(act);
        Assert.Equal(actual.Message, expected.Message);
    }

    [Fact]
    public void EqualityMembers_WorkAsExpected()
    {
        
    }
}