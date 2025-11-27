
using System;
using UserService.Domain.Exceptions.ValueObjects.Status;
using UserService.Domain.ValueObjects;
using static UserService.Domain.ValueObjects.Status;

namespace UserService.Tests.ValueObjects;

public class StatusTests
{
    [Fact]
    public void WithStatusType_SetValue()
    {
        var status = new Status(StatusType.Active);

        Assert.Equal(StatusType.Active, status.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void WithEmptyStatusValueString_ThrowMissingStatusValueException(string? input)
    {
        Assert.Throws<MissingStatusValueException>(() => new Status(input!));
    }

    [Fact]
    public void WithInvalidStatusValueString_ThrowUnsupportedStatusValueException()
    {
        Assert.Throws<UnsupportedStatusValueException>(() => new Role("not-a-status"));
    }
    
    [Theory]
    [InlineData("ACTIVE", StatusType.Active)]
    [InlineData(" Pending ", StatusType.Pending)]
    [InlineData("locked", StatusType.Locked)]
    public void WithValidStatusValueString_SetValue(string input, StatusType expected)
    {
        var status = new Status(input);

        Assert.Equal(expected, status.Value);
    }
}
