
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.FullName;

namespace UserService.Domain.ValueObjects;

public sealed record FullName : ValueObject<string>
{
    public override string Value { get; init; }

    public FullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new MissingFullNameException();

        string normalized = Normalize(fullName);
        if (!IsValid(normalized))
            throw new InvalidFullNameException(normalized);
        
        Value = normalized;
    }

    private string Validate

    private static string Normalize(string fullName)
        => fullName.Trim();

    private static bool IsValid(string fullName)
        => fullName.Length > 1 && fullName.Length < 100;

    public static FullName Parse(string fullName)
        => new(fullName);

    public override string ToString()
        => Value;
}
