
using System;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Exceptions.Status;

namespace AccountService.Domain.ValueObjects;

public sealed class Status : ValueObject
{
    public StatusType Value { get; }

    public enum StatusType
    {
        Pending,
        Active,
        Locked
    }

    public Status(StatusType value)
        => Value = value;

    public Status(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingStatusException();

        var normalized = value.Trim().ToLowerInvariant();
        if (!IsValid(normalized, out StatusType status))
            throw new UnsupportedStatusException(normalized);

        Value = status;
    }

    public static readonly Status Pending = new(StatusType.Pending);
    public static readonly Status Active = new(StatusType.Active);
    public static readonly Status Locked = new(StatusType.Locked);

    private static bool IsValid(string input, out StatusType result)
        => Enum.TryParse(input, ignoreCase: true, out result);

    public static Status From(string input)
        => new(input);

    public override string ToString()
        => Value.ToString();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
