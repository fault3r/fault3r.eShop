
using System;
using AccountService.Domain.Exceptions.Result;

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
            throw new MissingErrorMessageException();

        return new(false, error);
    }
}

public readonly struct Result<T>
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public T Value { get; }

    public bool IsFailure
        => !IsSuccess;

    private Result(bool isSuccess, T value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value)
    {
        if (value is null)
            throw new MissingValueException();

        return new(true, value, null);
    }

    public static Result<T> Failure(string error)
    {
        if (string.IsNullOrWhiteSpace(error))
            throw new MissingErrorMessageException();

        return new(false, default!, error);
    }
}