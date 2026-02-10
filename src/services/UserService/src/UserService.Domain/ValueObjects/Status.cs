
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
    }

    public StatusType Value { get; }

    private Status(StatusType status)
    {
        Value = status;
    }

    private Status(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingStatusException();

        value = value.Trim();

        if (!IsValid(value, out StatusType status))
            throw new UnsupportedStatusException(value);

        Value = status;
    }

    private static bool IsValid(string value, out StatusType status)
    {
        if (!Enum.TryParse(value, ignoreCase: true, out status))
            return false;

        return true;
    }

    public static Status From(StatusType status)
        => new(status);

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