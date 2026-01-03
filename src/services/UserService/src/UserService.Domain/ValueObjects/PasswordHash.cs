
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.PasswordHash;

namespace UserService.Domain.ValueObjects;

public sealed class PasswordHash : ValueObject<PasswordHash>
{
    public string Value { get; }

    private PasswordHash(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingPasswordHashException();

        value = value.Trim();

        if (!IsValid(value))
            throw new InvalidPasswordHashException(value);

        Value = value;
    }

    private static bool IsValid(string value)
        => value.Length > 50;

    public static PasswordHash From(string value)
        => new(value);

    public static bool TryFrom(string value, out PasswordHash? passwordHash)
    {
        try
        {
            passwordHash = From(value);
            return true;
        }
        catch
        {
            passwordHash = null;
            return false;
        }
    }

    public override string ToString()
        => Value;

    public static implicit operator string(PasswordHash hash)
        => hash.Value;

    public static explicit operator PasswordHash(string value)
        => From(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}