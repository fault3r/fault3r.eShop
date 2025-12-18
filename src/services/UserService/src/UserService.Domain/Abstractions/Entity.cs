
using System;
using UserService.Domain.Exceptions.Abstraction.Entity;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Abstractions;

public abstract class Entity<T, TId>
    : IEquatable<T>, IEntity, IEntity<TId>
    where T : Entity<T, TId>
{
    public TId Id { get; init; }

    protected Entity(TId id)
    {
        if (id is null)
            throw new MissingEntityIdentityException();

        Id = id;
    }

    public override string ToString()
        => $"{Id}";

    public override bool Equals(object? obj)
        => obj is T other && Equals(other);

    public bool Equals(T? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode()
        => HashCode.Combine(GetType(), Id);

    public static bool operator ==(Entity<T, TId>? left, Entity<T, TId>? right)
        => left?.Equals(right) ?? right is null;

    public static bool operator !=(Entity<T, TId>? left, Entity<T, TId>? right)
        => !(left == right);
}