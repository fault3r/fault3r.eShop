
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.PasswordSalt;

namespace UserService.Domain.ValueObjects;

public sealed class PasswordSalt : ValueObject<PasswordSalt>
{
    public string Value { get; }

    private PasswordSalt(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingPasswordSaltException();

        value = value.Trim();

        if (!IsValid(value))
            throw new InvalidPasswordSaltException(value);

        Value = value;
    }

    private static bool IsValid(string value)
        => value.Length >= 50
           && value.All(c =>
               (c >= 'A' && c <= 'Z') ||
               (c >= 'a' && c <= 'z') ||
               (c >= '0' && c <= '9')
            );

    public static PasswordSalt From(string value)
        => new(value);

    public static bool TryFrom(string value, out PasswordSalt? passwordSalt)
    {
        try
        {
            passwordSalt = From(value);
            return true;
        }
        catch
        {
            passwordSalt = null;
            return false;
        }
    }

    public override string ToString()
        => Value;

    public static implicit operator string(PasswordSalt passwordSalt)
        => passwordSalt.Value;

    public static explicit operator PasswordSalt(string value)
        => From(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
