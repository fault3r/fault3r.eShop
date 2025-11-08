
using System;
using AccountService.Domain.Exceptions;

namespace AccountService.Domain.Common;

public sealed record Identity
{
    public Guid Value { get; }

    public Identity(Guid value)
    {
        if (value == Guid.Empty)
            throw new EmptyGuidException();
        Value = value;
    }

    public Identity() : this(Guid.NewGuid()) { }

    public static Identity New() => new();

    public static Identity From(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new MissingGuidException();

        var normalized = input.Trim();
        if (!IsValid(normalized))
            throw new InvalidGuidException(normalized);

        return new Identity(Guid.Parse(normalized));
    }

    public static bool IsValid(string input)
        => Guid.TryParse(input, out var result) && result != Guid.Empty;

    public override string ToString() => Value.ToString();
}
