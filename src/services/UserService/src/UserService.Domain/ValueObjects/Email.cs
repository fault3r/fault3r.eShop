
using System;
using System.Net.Mail;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions;
using UserService.Domain.Exceptions.ValueObjects.Email;

namespace UserService.Domain.ValueObjects;

public sealed record Email : ValueObject<string>
{
    public override string Value { get; init; }

    private Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingEmailAddressException();

        if (!IsValid(value.Trim()))
            throw new InvalidEmailAddressException(value);

        Value = Normalize(value);
    }

    private static string Normalize(string value)
    {
        value = value.Trim();

        var atIndex = value.IndexOf('@');
        var local = value[..atIndex];
        var domain = value[(atIndex + 1)..];

        return $"{local}@{domain.ToLowerInvariant()}";
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
        catch (DomainException)
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
