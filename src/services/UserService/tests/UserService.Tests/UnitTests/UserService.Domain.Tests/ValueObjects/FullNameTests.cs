
using System;
using UserService.Domain.ValueObjects;
using UserService.Domain.Exceptions.FullName;

namespace UserService.Tests.UnitTests.UserService.Domain.Tests.ValueObjects;

public class FullNameTests
{
    [Fact]
    public void Parse_WithValidFullName_ReturnsFirstAndLastName()
    {
        var fullName = FullName.Parse("Gandalf the Grey");

        Assert.Equal("Gandalf", fullName.FirstName);
        Assert.Equal("the Grey", fullName.LastName);
        Assert.Equal("Gandalf the Grey", fullName.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Parse_WithMissingFullName_ThrowsMissingFullNameException(string? input)
    {
        Assert.Throws<MissingFullNameException>(() => FullName.Parse(input!));
    }

    [Theory]
    [InlineData("Gandalf")] 
    [InlineData("JR T")]
    public void Parse_WithInvalidFullName_ThrowsInvalidFullNameException(string input)
    {
        Assert.Throws<InvalidFullNameException>(() => FullName.Parse(input));
    }
}
