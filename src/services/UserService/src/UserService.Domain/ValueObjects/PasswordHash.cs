
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.PasswordHash;

namespace UserService.Domain.ValueObjects;

public sealed record PasswordHash : ValueObject<string>
{
    public override string Value { get; init; }

    private PasswordHash(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingPasswordHashValueException();

        var normalized = Normalize(value);

        if (!IsValid(normalized))
            throw new InvalidPasswordHashValueException(normalized);

        Value = normalized;
    }

    private static string Normalize(string value)
        => value.Trim();

    private static bool IsValid(string value)
        => value.StartsWith("$argon2id$") && value.Length >= 60;

    public static PasswordHash Parse(string value)
        => new(value);

    public static bool TryParse(string value, out PasswordHash? passwordHash)
    {
        try
        {
            passwordHash = new(value);
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
        => Parse(value);
}
