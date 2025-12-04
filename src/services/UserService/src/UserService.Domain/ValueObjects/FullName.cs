
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.FullName;

namespace UserService.Domain.ValueObjects;

public sealed record FullName : ValueObject<string>
{
    public override string Value { get; init; }

    private FullName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingFullNameException();

        var normalized = Normalize(value);

        if (!IsValid(normalized))
            throw new InvalidFullNameException(normalized);

        Value = normalized;
    }

    private static string Normalize(string value)
        => value.Trim();

    private static bool IsValid(string value)
        => value.Length > 1 && value.Length < 100;

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
