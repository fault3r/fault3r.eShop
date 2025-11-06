
using System;
using AccountService.Domain.Common;

namespace AccountService.Domain.Base;

public abstract class Entity(Identity id)
{
    public Identity Id { get; protected set; } = id;

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
        => Id.GetHashCode();
}
