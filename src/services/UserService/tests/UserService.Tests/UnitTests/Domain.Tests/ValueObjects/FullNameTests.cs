
using System;
using UserService.Domain.Exceptions.FullName;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.UnitTests.Domain.Tests.ValueObjects;

public class FullNameTests
{
    [Fact]
    public void Parse_WithValidFullName_ReturnsTrimmedFullName()
    {
        var fullname = FullName.Parse(" Hamed   Damaavandi  ");

        Assert.Equal("Hamed Damaavandi", fullname);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    [InlineData("")]
    public void Parse_WithMissingFullName_ThrowsMissingFullNameException(string? input)
    {
        FullName act() => FullName.Parse(input!);

        Assert.Throws<MissingFullNameException>(act);
    }

    [Theory]
    [InlineData("damaavandi")]
    [InlineData("fa n")]
    public void Parse_WithInvalidFullName_ThrowsInvalidFullNameException(string input)
    {
        FullName act() => FullName.Parse(input);

        Assert.Throws<InvalidFullNameException>(act);
    }
}
