
using System;

namespace AccountService.Domain.Common;

public readonly struct Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }

    public bool IsFailure
        => !IsSuccess;

    private Result(bool isSuccess, string? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success()
        => new(true, null);

    public static Result Failure(string error)
    {
        if (string.IsNullOrWhiteSpace(error))
            throw new Domain.Exceptions.DomainException("Failure result must have an error message.");

        return new(false, error);
    }
}