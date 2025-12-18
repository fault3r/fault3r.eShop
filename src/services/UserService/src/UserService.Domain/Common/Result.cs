
using System;
using UserService.Domain.Exceptions.Common.Result;

namespace UserService.Domain.Common;

public readonly struct Result<T>
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public T? Value { get; }

    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public bool IsFailure
        => !IsSuccess;

    public static Result<T> Success(T value)
    {
        if (value is null)
            throw new MissingResultValueException();

        return new(true, value, null);
    }

    public static Result<T> Failure(string error)
    {
        if (string.IsNullOrWhiteSpace(error))
            throw new MissingResultErrorMessageException();

        return new(false, default, error);
    }
}