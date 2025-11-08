
using System;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Exceptions.Identity;

namespace AccountService.Domain.ValueObjects;

public sealed class Identity : ValueObject
{
    public Guid Value { get; }

    public Identity()
        => Value = Guid.NewGuid();

    public Identity(Guid value)
    {
        if (value == Guid.Empty)
            throw new EmptyIdentityException();
        Value = value;
    }

    public Identity(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingIdentityException();

        var normalized = value.Trim();
        if (!IsValid(normalized))
            throw new InvalidIdentityException(normalized);

        Value = Guid.Parse(normalized);
    }

    public static Identity New()
        => new();

    public static Identity From(string input)
        => new(input);

    private static bool IsValid(string input)
        => Guid.TryParse(input, out var result)
        && result != Guid.Empty;

    public override string ToString()
        => Value.ToString();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
