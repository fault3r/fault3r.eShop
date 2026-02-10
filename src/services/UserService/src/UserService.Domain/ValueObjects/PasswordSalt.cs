
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
        => value.Length >= 8
           && value.All(c =>
               (c >= 'A' && c <= 'Z') ||
               (c >= 'a' && c <= 'z') ||
               (c >= '0' && c <= '9')
            );

    public static PasswordSalt Parse(string value)
        => new(value);

    public static bool TryParse(string value, out PasswordSalt? salt)
    {
        try
        {
            salt = Parse(value);
            return true;
        }
        catch
        {
            salt = null;
            return false;
        }
    }

    public override string ToString()
        => Value;

    public static implicit operator string(PasswordSalt salt)
        => salt.Value;

    public static explicit operator PasswordSalt(string value)
        => Parse(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
