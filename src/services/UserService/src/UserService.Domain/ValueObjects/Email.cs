
using System;
using System.Net.Mail;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.Email;

namespace UserService.Domain.ValueObjects;

public sealed record Email : ValueObject<string>
{
    public override string Value { get; init; }

    private Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingEmailAddressException();

        var normalized = Normalize(value);

        if (!IsValid(normalized))
            throw new InvalidEmailAddressException(normalized);

        Value = normalized;
    }

    private static string Normalize(string value)
    {
        var trimmed = value.Trim();

        var parts = trimmed.Split('@');
        if (parts.Length != 2)
            return trimmed.ToLowerInvariant();

        return $"{parts[0]}@{parts[1].ToLowerInvariant()}";
    }

    private static bool IsValid(string value)
       => MailAddress.TryCreate(value, out _);

    public static Email Parse(string value)
        => new(value);

    public static bool TryParse(string value, out Email? email)
    {
        try
        {
            email = new(value);
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
        => Parse(value);
}
