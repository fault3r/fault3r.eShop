
using System;

namespace UserService.Domain.Common;

public readonly struct Result
{
    public bool IsSuccess { get; init; }
    public string? Error { get; init; }

    private Result(bool isSuccess, string? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsFailure => !IsSuccess;

    public static Result Success()
        => new(true, null);

    public static Result Failure(string error)
    {
        ArgumentException.ThrowIfNullOrEmpty(error);

        return new(false, error);
    }
}

public readonly struct Result<T>
    where T : class
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public T? Value { get; }

    private Result(bool isSuccess, string? error, T? value)
    {
        IsSuccess = isSuccess;
        Error = error;
        Value = value;
    }

    public bool IsFailure => !IsSuccess;

    public static Result<T> Success(T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new(true, null, value);
    }

    public static Result<T> Failure(string error)
    {
        ArgumentException.ThrowIfNullOrEmpty(error);

        return new(false, error, null);
    }
}

