
using System;
using UserService.Domain.Exceptions.ValueObjects.Identity;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.ValueObjects;

public class IdentityTests
{
    [Fact]
    public void WithNoArgs_GenerateNewGuid()
    {
        var identity = new Identity();

        Assert.NotEqual(Guid.Empty, identity.Value);
    }

    [Fact]
    public void WithEmptyGuid_ThrowEmptyIdentityValueException()
    {
        Assert.Throws<EmptyIdentityValueException>(() => new Identity(Guid.Empty));
    }

    [Fact]
    public void WithValidGuidProvided_SetValue()
    {
        var guid = Guid.NewGuid();

        var identity = new Identity(guid);

        Assert.Equal(guid, identity.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void WithEmptyGuidString_ThrowEmptyIdentityValueException(string input)
    {
        Assert.Throws<EmptyIdentityValueException>(() => new Identity(input));
    }


    [Fact]
    public void WithValidGuidString_SetValue()
    {
        var guid = Guid.NewGuid();
        var guidString = guid.ToString();

        var identity = new Identity(guidString);

        Assert.Equal(guid, identity.Value);
    }

    [Fact]
    public void WithInvalidGuidString_ThrowInvalidIdentityValueException()
    {
        Assert.Throws<InvalidIdentityValueException>(() => new Identity("not-a-guid"));
        Assert.Throws<InvalidIdentityValueException>(() => new Identity("00000000-0000-0000-0000-000000000000"));
    }
}

