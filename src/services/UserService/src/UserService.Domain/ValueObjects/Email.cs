
using System;
using System.Text.RegularExpressions;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.Email;

namespace UserService.Domain.ValueObjects;

public sealed class Email : ValueObject<Email>
{
    public string Value { get; }

    private Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingEmailException();

        value = value.Trim();

        if (!IsValid(value))
            throw new InvalidEmailException(value);

        Value = Normalize(value);
    }

    private static readonly Regex EmailRegex = new(
        pattern: @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        options: RegexOptions.Compiled | RegexOptions.CultureInvariant
    );

    private static bool IsValid(string value)
        => EmailRegex.IsMatch(value);

    private static string Normalize(string value)
    {
        var atIndex = value.IndexOf('@');
        var local = value[..atIndex];
        var domain = value[++atIndex..];

        return $"{local}@{domain.ToLowerInvariant()}";
    }

    public static Email From(string value)
        => new(value);

    public static bool TryFrom(string value, out Email? email)
    {
        try
        {
            email = From(value);
            return true;
        }
        catch
        {
            email = null;
            return false;
        }
    }

    public override string ToString()
        => Value;

    public static implicit operator string(Email email)
        => email.Value;

    public static explicit operator Email(string value)
        => From(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
