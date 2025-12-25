
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Common;
using UserService.Domain.Exceptions;
using UserService.Domain.Exceptions.ValueObjects.PasswordHash;

namespace UserService.Domain.ValueObjects;

public sealed class PasswordHash : ValueObject<PasswordHash>
{
    public string Value { get; }

    private PasswordHash(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingPasswordHashValueException();

        value = value.Trim();

        if (!IsValid(value))
            throw new InvalidPasswordHashValueException(value);

        Value = value;
    }

    private static bool IsValid(string value)
        => value.StartsWith("$argon2id$") && value.Length > 60;

    public static PasswordHash Parse(string value)
        => new(value);

    public static Result<PasswordHash> TryParse(string value, out PasswordHash? passwordHash)
    {
        try
        {
            passwordHash = new(value);
            return Result<PasswordHash>.Success(passwordHash);
        }
        catch (PasswordHashException ex)
        {
            passwordHash = null;
            return Result<PasswordHash>.Failure(ex.Message);
        }
    }

    public override string ToString()
        => Value;

    public static implicit operator string(PasswordHash hash)
        => hash.Value;

    public static explicit operator PasswordHash(string value)
        => Parse(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
