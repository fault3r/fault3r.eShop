
using System;
using System.Net.Mail;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions;
using UserService.Domain.Exceptions.ValueObjects.Email;

namespace UserService.Domain.ValueObjects;

public sealed class Email : ValueObject<Email>
{
    public string Value { get; }

    private Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingEmailAddressException();

        if (!IsValid(value.Trim()))
            throw new InvalidEmailAddressException(value);

        Value = Normalize(value);
    }

    private static bool IsValid(string value)
       => MailAddress.TryCreate(value, out _);
       
    private static string Normalize(string value)
    {
        value = value.Trim();

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
        => From(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
