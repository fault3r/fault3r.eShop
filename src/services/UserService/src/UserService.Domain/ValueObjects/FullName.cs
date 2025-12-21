
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions;
using UserService.Domain.Exceptions.ValueObjects.FullName;

namespace UserService.Domain.ValueObjects;

public sealed record FullName : ValueObject<string>
{
    public override string Value { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }

    private FullName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingFullNameException();

        value = value.Trim();

        if (!IsValid(value))
            throw new InvalidFullNameException(value);

        var parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        FirstName = parts[0];
        LastName = string.Join(" ", parts.Skip(1));
        Value = $"{FirstName} {LastName}";
    }

    private static bool IsValid(string value)
    {
        if (value.Length < 2 || value.Length > 99)
            return false;

        var parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
            return false;

        return true;
    }

    public static FullName Parse(string value)
        => new(value);

    public static bool TryParse(string value, out FullName? fullName)
    {
        try
        {
            fullName = new(value);
            return true;
        }
        catch (DomainException)
        {
            fullName = null;
            return false;
        }
    }

    public override string ToString()
        => Value;

    public static implicit operator string(FullName fullName)
        => fullName.Value;

    public static explicit operator FullName(string value)
        => Parse(value);
}
