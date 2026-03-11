
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.Identity;

namespace UserService.Domain.ValueObjects;

public sealed class Identity : ValueObject<Identity>
{
    public Guid Value { get; }

    private Identity(Guid guid)
    {
        if (guid == Guid.Empty)
            throw new EmptyIdentityException();

        Value = guid;
    }

    private Identity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new EmptyIdentityException();

        value = value.Trim();

        if (!IsValid(value))
            throw new InvalidIdentityException(value);

        Value = Guid.Parse(value);
    }

    private static bool IsValid(string value)
        => Guid.TryParse(value, out var guid) && guid != Guid.Empty;

    public static Identity From(Guid guid)
        => new(guid);

    public static Identity Parse(string value)
        => new(value);

    public static bool TryParse(string value, out Identity? identity)
    {
        try
        {
            identity = Parse(value);
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
        => identity.ToString();

    public static explicit operator Identity(string value)
        => Parse(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
