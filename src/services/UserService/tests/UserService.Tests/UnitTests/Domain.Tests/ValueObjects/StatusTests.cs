
using System;
using UserService.Domain.Exceptions.Status;
using UserService.Domain.ValueObjects;

namespace UserService.Tests.UnitTests.Domain.Tests.ValueObjects;

public class StatusTests
{
    [Theory]
    [InlineData("Locked")]
    [InlineData("Pending")]
    [InlineData("Active")]
    [InlineData("Unsupported")]
    public void Parse_WithValidStatus_ReturnsStatus(string input)
    {
        var status = Status.Parse(input);

        Assert.Equal(input.ToLower(), status.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    [InlineData("")]
    public void Parse_WithMissingStatus_ThrowsMissingStatusException(string? input)
    {
        Status act() => Status.Parse(input!);

        Assert.Throws<MissingStatusException>(act);
    }

    [Fact]
    public void Parse_WithInvalidStatus_ThrowsUnsupportedStatusException()
    {
        Status act() => Status.Parse("invalid");

        Assert.Throws<UnsupportedStatusException>(act);
    }
}
