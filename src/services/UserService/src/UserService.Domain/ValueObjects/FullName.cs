
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Common;
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

        if (!IsValid(value.Trim()))
            throw new InvalidFullNameException(value);

        var parts = value
            .Trim()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        FirstName = parts[0];
        LastName = string.Join(" ", parts.Skip(1));
    }

    private static bool IsValid(string value)
    {
        if (value.Length < 1 + 1 || value.Length > 101)
            return false;

        var parts = value
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2) return false;

        return true;
    }

    public static FullName From(string value)
        => new(value);

    public static Result<FullName> TryFrom(string value, out FullName? fullName)
    {
        try
        {
            fullName = new(value);
            return Result<FullName>.Success(fullName);

        }
        catch (FullNameException ex)
        {
            fullName = null;
            return Result<FullName>.Failure(ex.Message);
        }
    }

    public override string ToString()
        => $"{FirstName} {LastName}";

    public static implicit operator string(FullName fullName)
        => fullName.ToString();

    public static explicit operator FullName(string value)
        => From(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }
}
