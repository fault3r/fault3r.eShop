using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.Status;

namespace UserService.Domain.ValueObjects;

public sealed class Status : ValueObject<Status>
{
    public StatusType Value { get; }

    public enum StatusType
    {
        Locked = -1,
        Pending = 0,
        Active = 1,
    }

    public Status(StatusType statusType)
        => Value = statusType;

    public Status(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingStatusValueException();

        var normalized = value
            .Trim()
            .ToLowerInvariant();
        if (!IsValid(normalized, out StatusType status))
            throw new UnsupportedStatusValueException(normalized);

        Value = status;
    }

    public static readonly Status Pending = new(StatusType.Pending);
    public static readonly Status Active = new(StatusType.Active);
    public static readonly Status Locked = new(StatusType.Locked);

    private static bool IsValid(string value, out StatusType result)
        => Enum.TryParse(value, ignoreCase: true, out result);

    public static Status From(StatusType statusType)
        => new(statusType);

    public static Status Parse(string value)
        => new(value);

    public override string ToString()
        => Value.ToString();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}