using System;
using UserService.Domain.ValueObjects;
using UserService.Domain.Exceptions.ValueObjects.FullName;
using Xunit;

namespace UserService.Tests.ValueObjects;

public class FullNameTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void WithEmptyFullNameString_ThrowMissingFullNameException(string? input)
    {
        Assert.Throws<MissingFullNameException>(() => new FullName(input!));
    }

    [Fact]
    public void WithInvalidFullNameString_ThrowInvalidFullNameException()
    {
        Assert.Throws<InvalidFullNameException>(() => new FullName("T"));
    }

    [Fact]
    public void WithValidFullName_SetValue()
    {
        var fullName = new FullName("example");

        Assert.Equal("example", fullName.ToString());
    }
}