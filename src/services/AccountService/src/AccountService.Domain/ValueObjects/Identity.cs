
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
            throw new EmptyGuidException();
        Value = value;
    }

    public Identity(string guid)
    {
        if (string.IsNullOrWhiteSpace(guid))
            throw new MissingGuidException();

        var normalized = guid.Trim();
        if (!IsValid(normalized))
            throw new InvalidGuidException(normalized);

        Value = Guid.Parse(normalized);
    }

    public static Identity New()
        => new();

    public static Identity From(string input)
        => new(input);

    public static bool IsValid(string input)
        => Guid.TryParse(input, out var result)
        && result != Guid.Empty;

    public override string ToString()
        => Value.ToString();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
