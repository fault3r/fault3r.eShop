
using System;
using System.Net.Mail;
using AccountService.Domain.Exceptions;

namespace AccountService.Domain.Accounts;

public sealed class Email : IEquatable<Email>
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email From(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new DomainException("Email is required");
        var normalized = value.Trim().ToLowerInvariant();
        if (!IsValid(normalized))
            throw new DomainException($"Invalid email: {normalized}");
        return new Email(normalized);
    }

    private static bool IsValid(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch { return false; }
    }

    public override string ToString() 
        => Value;

    public bool Equals(Email? other)
        => other is not null && other.Value == Value;

    public override bool Equals(object? obj)
        => obj is Email && Equals(obj as Email);

    public override int GetHashCode()
        => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);

    public static explicit operator Email(string value)
        => From(value);

    public static implicit operator string(Email email)
        => email.Value;
}
