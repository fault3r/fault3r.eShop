
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.Status;

namespace UserService.Domain.ValueObjects;

public sealed class Status : ValueObject<Status>
{
    public enum StatusType
    {
        Locked = 1,
        Pending = 2,
        Active = 3,
        Unsupported = 0,
    }

    public StatusType Value { get; }

    private Status(StatusType statusType)
        => Value = statusType;

    private Status(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingStatusException();

        value = value.Trim();

        if (!TryGetValue(value, out StatusType status))
            throw new UnsupportedStatusException(value);

        Value = status;
    }

    private static bool TryGetValue(string value, out StatusType statusType)
    {
        if (!Enum.TryParse(value, ignoreCase: true, out statusType))
            return false;

        return true;
    }

    public static Status From(StatusType statusType)
        => new(statusType);

    public static Status Parse(string value)
        => new(value);

    public static bool TryParse(string value, out Status? status)
    {
        try
        {
            status = Parse(value);
            return true;
        }
        catch
        {
            status = null;
            return false;
        }
    }

    public bool IsLocked => Value == StatusType.Locked;
    public bool IsPending => Value == StatusType.Pending;
    public bool IsActive => Value == StatusType.Active;

    public override string ToString()
        => Value.ToString().ToLower();

    public static implicit operator string(Status status)
        => status.ToString();

    public static explicit operator Status(string value)
        => Parse(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static readonly Status Locked = new(StatusType.Locked);
    public static readonly Status Pending = new(StatusType.Pending);
    public static readonly Status Active = new(StatusType.Active);
}