
using System;
using UserService.Domain.Exceptions.ValueObjects.Identity;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.ValueObjects;

public class IdentityTests
{
    [Fact]
    public void Constructor_WhenNoArgs_ShouldGenerateNewGuid()
    {
        var identity = new Identity();

        Assert.NotEqual(Guid.Empty, identity.Value);
    }

    [Fact]
    public void Constructor_WithEmptyGuid_ShouldThrowException()
    {
        Assert.Throws<EmptyIdentityValueException>(() => new Identity(Guid.Empty));
    }

    [Fact]
    public void Constructor_WithValidGuidProvided_ShouldSetValue()
    {
        var guid = Guid.NewGuid();

        var identity = new Identity(guid);

        Assert.Equal(guid, identity.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyString_ShouldThrowException(string input)
    {
        Assert.Throws<EmptyIdentityValueException>(() => new Identity(input));
    }


    [Fact]
    public void Constructor_ShouldParseValidGuidString()
    {
        var guid = Guid.NewGuid();
        var guidString = guid.ToString();

        var identity = new Identity(guidString);

        Assert.Equal(guid, identity.Value);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenInvalidGuidStringProvided()
    {
        Assert.Throws<InvalidIdentityValueException>(() => new Identity("not-a-guid"));
        Assert.Throws<InvalidIdentityValueException>(() => new Identity("00000000-0000-0000-0000-000000000000"));
    }
}

