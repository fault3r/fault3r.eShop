
using System;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Abstractions;

public abstract class Entity
{
    protected Identity Id { get; set; }

    protected Entity()
        => Id = Identity.New();

    protected Entity(Identity id)
        => Id = id;

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
        => Id.GetHashCode();
}