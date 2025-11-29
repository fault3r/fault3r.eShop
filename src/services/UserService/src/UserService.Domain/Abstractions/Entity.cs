
using System;
using UserService.Domain.Exceptions.Abstraction.Entity;

namespace UserService.Domain.Abstractions;

public abstract class Entity<TType, TId> : IEquatable<Entity<TType, TId>>
    where TType : Entity<TType, TId>
{
    public TId Id { get; protected set; }

    protected Entity(TId id)
    {
        if (id is null)
            throw new MissingEntityIdentityException();

        Id = id;
    }

    public override string ToString()
        => Id is null
            ? $"{typeof(TId).Name}"
            : $"{typeof(TId).Name}:{Id.ToString()}";


    public override bool Equals(object? obj)
        => Equals(obj as Entity<TType, TId>);

    public bool Equals(Entity<TType, TId>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode()
        => HashCode.Combine(GetType(), Id);

    public static bool operator ==(Entity<TType, TId>? left, Entity<TType, TId>? right)
        => left?.Equals(right) ?? right is null;

    public static bool operator !=(Entity<TType, TId>? left, Entity<TType, TId>? right)
        => !(left == right);
}