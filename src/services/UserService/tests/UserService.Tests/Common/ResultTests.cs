
using System;
using UserService.Domain.Common;
using UserService.Domain.Exceptions.Common.Result;

namespace UserService.Tests.Common;

public class ResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessfulResult()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Failure_ShouldCreateFailureResult()
    {
        var result = Result.Failure("something went wrong");

        Assert.False(result.IsSuccess);
        Assert.Equal("something went wrong", result.Error);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Failure_WithEmptyFailureMessage_ThrowMissingResultErrorMessageException(string? input)
    {
        Assert.Throws<MissingResultErrorMessageException>(() => Result.Failure(input!));
    }
}