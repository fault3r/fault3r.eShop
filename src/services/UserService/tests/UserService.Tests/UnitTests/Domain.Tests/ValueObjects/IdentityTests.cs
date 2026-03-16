
using System;
using UserService.Domain.Exceptions.Identity;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.UnitTests.Domain.Tests.ValueObjects;

public class IdentityTests
{
    [Fact]
    public void ParseFrom_WithValidIdentity_ReturnsIdentity()
    {
        var guid = Guid.NewGuid();
        var identityFrom = Identity.From(guid);
        var identityParse = Identity.Parse(guid.ToString());

        Assert.Equal(identityFrom.Value, guid);
        Assert.Equal(identityFrom, identityParse);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    [InlineData("")]
    public void Parse_WithMissingIdentity_ThrowsMissingIdentityException(string? input)
    {
        Identity act() => Identity.Parse(input!);

        Assert.Throws<MissingIdentityException>(act);
    }

    [Theory]
    [InlineData("invalid-guid")]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void Parse_WithInvalidFullName_ThrowsInvalidIdentityException(string input)
    {
        Identity act() => Identity.Parse(input);

        Assert.Throws<InvalidIdentityException>(act);
    }
}
