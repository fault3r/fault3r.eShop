
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.FullName;

namespace UserService.Domain.ValueObjects;

public sealed class FullName : ValueObject<FullName>
{
    public string Value { get; }

    public FullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new MissingFullNameException();

        var normalized = fullName.Trim();
        if (!IsValid(normalized))
            throw new InvalidFullNameException(normalized);
        
        Value = normalized;
    }

    private static bool IsValid(string fullName)
        => fullName.Length > 1 && fullName.Length < 100;

    public static FullName Parse(string fullName)
        => new(fullName);

    public override string ToString()
        => Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
