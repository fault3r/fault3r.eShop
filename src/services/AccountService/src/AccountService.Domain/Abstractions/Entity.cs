
using System;
using AccountService.Domain.Exceptions.Identity;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Abstractions;

public abstract class Entity
{
    public Identity Id { get; protected set; }

    protected Entity(Identity id)
        => Id = id
            ?? throw new MissingIdentityException();

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
        => Id.GetHashCode();

    public override string ToString()
        => Id.ToString();
}