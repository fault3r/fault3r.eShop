
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Common;
using UserService.Domain.Exceptions.Status;

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

    private Status(StatusType statusType)
    {
        Value = statusType;
    }

    private Status(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingStatusException();

        value = value.Trim();

        if (!TryParse(value, out StatusType status))
            throw new UnsupportedStatusException(value);

        Value = status;
    }
    
    public static readonly Status Locked = new(StatusType.Locked);
    public static readonly Status Pending = new(StatusType.Pending);
    public static readonly Status Active = new(StatusType.Active);

    private static bool TryParse(string value, out StatusType statusType)
    {
        if (!Enum.TryParse(value, ignoreCase: true, out statusType))
            return false;

        return Enum.IsDefined(typeof(StatusType), statusType);
    }

    public static Status From(StatusType statusType)
        => new(statusType);

    public static Status From(string value)
        => new(value);

    public static Result<Status> TryFrom(string value, out Status? status)
    {
        try
        {
            status = new(value);
            return Result<Status>.Success(status);
        }
        catch (StatusException ex)
        {
            status = null;
            return Result<Status>.Failure(ex.Message);
        }
    }

    public bool IsLocked => Value == StatusType.Locked;
    public bool IsPending => Value == StatusType.Pending;
    public bool IsActive => Value == StatusType.Active;

    public override string ToString()
        => Value.ToString().ToLowerInvariant();

    public static implicit operator string(Status status)
        => status.ToString();

    public static explicit operator Status(string value)
        => From(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

}