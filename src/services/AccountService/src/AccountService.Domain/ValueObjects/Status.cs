
using System;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Exceptions;

namespace AccountService.Domain.ValueObjects;

public sealed class Status : ValueObject
{
    public StatusType Value { get; private set; }

    public enum StatusType
    {
        Pending,
        Active,
        Locked
    }

    private Status(StatusType value)
        => Value = value;

    public static Status Pending => new(StatusType.Pending);
    public static Status Active => new(StatusType.Active);
    public static Status Locked => new(StatusType.Locked);

    public static IReadOnlyCollection<string> AllowedStatuses { get; }
        = [nameof(Pending), nameof(Active), nameof(Locked)];

    public static IEnumerable<string> GetAllowedStatuses()
        => AllowedStatuses;

    public static Status From(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new MissingStatusException();
        var normalized = value.Trim();
        return normalized switch
        {
            nameof(Pending) => new(StatusType.Pending),
            nameof(Active) => new(StatusType.Active),
            nameof(Locked) => new(StatusType.Locked),
            _ => throw new UnsupportedStatusException(normalized)
        };
    }

    public override string ToString()
        => Value.ToString();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
