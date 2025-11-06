
using System;

namespace AccountService.Domain.Common;

public readonly struct Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Message { get; }

    private Result(bool isSuccess, string? message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public static Result Success(string? message)
        => new(true, message);

    public static Result Failure(string? error)
        => new(false, error);
}

public readonly struct Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Message { get; }
    public T? Value { get; }
    
    private Result(bool isSuccess, string? message, T? value)
    {
        IsSuccess = isSuccess;
        Message = message;
        Value = value;
    }

    public static Result<T> Success(string? message, T? value)
        => new(true, message, value);

    public static Result<T> Failure(string? error, T? value)
        => new(false, error, value);
}
