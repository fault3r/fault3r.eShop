
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

    public static Identity New() => new();

    public static Identity From(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new MissingGuidException();

        var normalized = input.Trim();
        if (!Guid.TryParse(normalized, out var @out))
            throw new InvalidGuidException(normalized);

        return new(Guid.Parse(normalized));
    }

    public static bool IsValid(string input)
        => Guid.TryParse(input, out var result) && result != Guid.Empty;

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
