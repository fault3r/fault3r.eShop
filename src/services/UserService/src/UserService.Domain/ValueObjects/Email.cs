
using System;
using System.Text.RegularExpressions;
using UserService.Domain.Abstractions;
using UserService.Domain.Common;
using UserService.Domain.Exceptions.Email;

namespace UserService.Domain.ValueObjects;

public sealed partial class Email : ValueObject<Email>
{
    public string Value { get; }

    private Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingEmailException();

        if (!IsValid(value.Trim()))
            throw new InvalidEmailException(value);

        Value = Normalize(value);
    }

    private static bool IsValid(string value)
       => EmailRegex().IsMatch(value);

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

    public static Result<Email> TryFrom(string value, out Email? email)
    {
        try
        {
            email = new(value);
            return Result<Email>.Success(email);
        }
        catch (EmailException ex)
        {
            email = null;
            return Result<Email>.Failure(ex.Message);
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

    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex EmailRegex();
}
