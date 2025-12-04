
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.Identity;

namespace UserService.Domain.ValueObjects;

public sealed record Identity : ValueObject<Guid>
{
    public override Guid Value { get; init; }

    private Identity(Guid guid)
    {
        if (guid == Guid.Empty)
            throw new EmptyIdentityValueException();

        Value = guid;
    }

    private Identity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new EmptyIdentityValueException();

        var normalized = Normalize(value);

        if (!IsValid(normalized))
            throw new InvalidIdentityValueException(normalized);

        Value = Guid.Parse(normalized);
    }

    private static string Normalize(string value)
        => value.Trim();

    private static bool IsValid(string value)
        => Guid.TryParse(value, out var guid) && guid != Guid.Empty;

    public static Identity New()
        => new(Guid.NewGuid());

    public static Identity From(Guid guid)
        => new(guid);

    public static Identity Parse(string value)
        => new(value);

    public static bool TryParse(string value, out Identity? identity)
    {
        try
        {
            identity = new(value);
            return true;
        }
        catch
        {
            identity = null;
            return false;
        }
    }

    public override string ToString()
        => Value.ToString();

    public static implicit operator string(Identity identity)
        => identity.Value.ToString();

    public static explicit operator Identity(string value)
        => Parse(value);
}
