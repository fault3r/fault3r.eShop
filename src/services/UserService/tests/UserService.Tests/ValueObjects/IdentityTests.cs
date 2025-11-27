
using System;
using UserService.Domain.Exceptions.ValueObjects.Identity;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.ValueObjects;

public class IdentityTests
{
    [Fact]
    public void Constructor_ShouldGenerateNewGuid_WhenNoArgs()
    {
        var identity = new Identity();

        Assert.NotEqual(Guid.Empty, identity.Value);
    }

    [Fact]
    public void Constructor_ShouldSetGuid_WhenValidGuidProvided()
    {
        var guid = Guid.NewGuid();

        var identity = new Identity(guid);

        Assert.Equal(guid, identity.Value);
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
    public void Constructor_ShouldThrow_WhenEmptyGuidProvided()
    {
        Assert.Throws<EmptyIdentityValueException>(() => new Identity(Guid.Empty));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrow_WhenEmptyStringProvided(string input)
    {
        Assert.Throws<EmptyIdentityValueException>(() => new Identity(input));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenInvalidGuidStringProvided()
    {
        Assert.Throws<InvalidIdentityValueException>(() => new Identity("not-a-guid"));
    }
}

