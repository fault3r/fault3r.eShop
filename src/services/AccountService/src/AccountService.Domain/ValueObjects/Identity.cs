
using System;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Exceptions.Identity;

namespace AccountService.Domain.ValueObjects;

public sealed class Identity : ValueObject
{
    public Guid Id { get; }

    public Identity()
        => Id = Guid.NewGuid();

    public Identity(Guid id)
    {
        if (id == Guid.Empty)
            throw new EmptyGuidException();
        Id = id;
    }

    public Identity(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new MissingGuidException();

        var normalized = id.Trim();
        if (!IsValid(normalized))
            throw new InvalidGuidException(normalized);

        Id = Guid.Parse(normalized);
    }

    public static Identity New()
        => new();

    public static Identity From(string input)
        => new(input);

    private static bool IsValid(string input)
        => Guid.TryParse(input, out var result)
        && result != Guid.Empty;

    public override string ToString()
        => Id.ToString();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }
}
