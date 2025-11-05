
using System;

namespace AccountService.Domain.Common;

public readonly struct Result
{
    public bool IsSuccess { get; }
    public string? Message { get; }

    private Result(bool isSuccess, string? message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public static Result Ok()
        => new(true, null);

    public static Result Fail(string error)
        => new(false, error);
}
