
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.Identity;

namespace UserService.Domain.ValueObjects;

public sealed class Identity : ValueObject
{
    public Guid Value { get; }

    public Identity()
        => Value = Guid.NewGuid();

    public Identity(Guid value)
    {
        if (value == Guid.Empty)
            throw new EmptyIdentityGuidException();

        Value = value;
    }

    public Identity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new EmptyIdentityGuidException();

        var normalized = value.Trim();
        if (!IsValid(normalized))
            throw new InvalidIdentityValueException(normalized);

        Value = Guid.Parse(normalized);
    }

    private static bool IsValid(string input)
        => Guid.TryParse(input, out var result) && result != Guid.Empty;

    public static Identity New() => new();
    public static Identity From(Guid guid) => new(guid);
    public static Identity Parse(string input) => new(input);

    public override string ToString()
        => Value.ToString();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
