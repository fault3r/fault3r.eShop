
using System;
using UserService.Domain.Common;
using UserService.Domain.Exceptions.Common.Result;

namespace UserService.Tests.Common;

public class ResultGenericTests
{
    [Fact]
    public void Success_WithNullValue_ThrowMissingResultValueException()
    {
        Assert.Throws<MissingResultValueException>(() => Result<string>.Success(null!));
    }

    [Fact]
    public void Success_ShouldCreateSuccessfulResult()
    {
        var result = Result<string>.Success("example");

        Assert.True(result.IsSuccess);
        Assert.Equal("example", result.Value);
        Assert.Null(result.Error);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Failure_WithEmptyFailureMessage_ThrowMissingResultErrorMessageException(string? input)
    {
        Assert.Throws<MissingResultErrorMessageException>(() => Result<string>.Failure(input!));
    }

    [Fact]
    public void Failure_ShouldCreateFailureResult()
    {
        var result = Result<string>.Failure("something went wrong");

        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.Equal("something went wrong", result.Error);
    }
}