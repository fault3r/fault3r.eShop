
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions;
using UserService.Domain.Exceptions.ValueObjects.Status;
using static UserService.Domain.ValueObjects.Status;

namespace UserService.Domain.ValueObjects;

public sealed record Status : ValueObject<StatusType>
{
    public override StatusType Value { get; init; }

    public enum StatusType
    {
        Locked = -1,
        Pending = 0,
        Active = 1,
    }

    private Status(StatusType statusType)
    {
        Value = statusType;
    }

    private Status(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingStatusValueException();

        value = value.Trim();

        if (!IsValid(value, out StatusType status))
            throw new UnsupportedStatusValueException(value);

        Value = status;
    }
    
    public static readonly Status Locked = new(StatusType.Locked);
    public static readonly Status Pending = new(StatusType.Pending);
    public static readonly Status Active = new(StatusType.Active);

    private static bool IsValid(string value, out StatusType statusType)
        => Enum.TryParse(value, ignoreCase: true, out statusType);

    public static Status From(StatusType statusType)
        => new(statusType);

    public static Status Parse(string value)
        => new(value);

    public static bool TryParse(string value, out Status? status)
    {
        try
        {
            status = new(value);
            return true;
        }
        catch (DomainException)
        {
            status = null;
            return false;
        }
    }

    public bool IsLocked => Value == StatusType.Locked;
    public bool IsPending => Value == StatusType.Pending;
    public bool IsActive => Value == StatusType.Active;

    public override string ToString()
        => Value.ToString();

    public static implicit operator string(Status status)
        => status.Value.ToString();

    public static explicit operator Status(string value)
        => Parse(value);
}