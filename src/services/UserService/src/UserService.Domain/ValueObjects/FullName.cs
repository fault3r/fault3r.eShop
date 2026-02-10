
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.FullName;

namespace UserService.Domain.ValueObjects;

public sealed class FullName : ValueObject<FullName>
{
    public string FirstName { get; }
    public string LastName { get; }

    private FullName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingFullNameException();

        value = value.Trim();

        if (!IsValid(value))
            throw new InvalidFullNameException(value);

        var parts = value
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        FirstName = parts[0];
        LastName = string.Join(' ', parts.Skip(1));
    }

    private static bool IsValid(string value)
    {
        if (value.Length >= 2 && value.Length <= 100)
        {
            var parts = value
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 1) return true;
        }

        return false;
    }

    public static FullName From(string firstname, string lastname)
        => new($"{firstname} {lastname}");

    public static FullName Parse(string value)
        => new(value);

    public static bool TryParse(string value, out FullName? fullName)
    {
        try
        {
            fullName = Parse(value);
            return true;
        }
        catch
        {
            fullName = null;
            return false;
        }
    }

    public override string ToString()
        => $"{FirstName} {LastName}";

    public static implicit operator string(FullName fullName)
        => fullName.ToString();

    public static explicit operator FullName(string value)
        => Parse(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }

    #region ⤚EFCore
    private FullName()
    {
        FirstName = null!;
        LastName = null!;
    }
    #endregion
}
