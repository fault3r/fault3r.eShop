
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.Identity;

namespace UserService.Domain.ValueObjects;

public sealed record Identity : ValueObject<Guid>
{
    public override Guid Value { get; init; }

    public Identity()
        => Value = Guid.NewGuid();

    public Identity(Guid guid)
    {
        if (guid == Guid.Empty)
            throw new EmptyIdentityValueException();

        Value = guid;
    }

    public Identity(string guidString)
    {
        if (string.IsNullOrWhiteSpace(guidString))
            throw new EmptyIdentityValueException();

        var normalized = Normalize(guidString);
        if (!IsValid(normalized))
            throw new InvalidIdentityValueException(normalized);

        Value = Guid.Parse(normalized);
    }

    private static string Normalize(string guidString)
        => guidString.Trim();

    private static bool IsValid(string guidString)
        => Guid.TryParse(guidString, out var guid) && guid != Guid.Empty;

    public static Identity New()
        => new();

    public static Identity From(Guid guid)
        => new(guid);

    public static Identity Parse(string guidString)
        => new(guidString);

    public override string ToString()
        => Value.ToString();
}
