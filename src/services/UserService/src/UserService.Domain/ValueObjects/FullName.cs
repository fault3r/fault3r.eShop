
using System;
using UserService.Domain.Abstractions;
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
        
        var normalized = Normalize(value);   
        var parts = normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2)
            throw new InvalidFullNameException(normalized);

        var normalizedFirstName = parts[0];
        var normalizedLastName = string.Join(" ", parts.Skip(1));
        if (!IsValid(normalizedFirstName) || !IsValid(normalizedLastName))
            throw new InvalidFullNameException(normalized);

        FirstName = normalizedFirstName;
        LastName = normalizedLastName;
        Value =  $"{FirstName} {LastName}";
    }

    private FullName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            throw new MissingFullNameException();

        var normalizedFirstName = Normalize(firstName);
        var normalizedLastName = Normalize(lastName);

        if (!IsValid(normalizedFirstName) || !IsValid(normalizedLastName))
            throw new InvalidFullNameException($"{normalizedFirstName} {normalizedLastName}");

        FirstName = normalizedFirstName;
        LastName = normalizedLastName;
        Value = $"{FirstName} {LastName}";
    }

    private static string Normalize(string value)
        => value.Trim();

    private static bool IsValid(string value)
        => value.Length > 1 && value.Length < 50;

    public static FullName From(string firstName, string lastName)
        => new(firstName, lastName);

    public static FullName Parse(string value)
        => new(value);

    public static bool TryParse(string value, out FullName? fullName)
    {
        try
        {
            fullName = new(value);
            return true;
        }
        catch
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
