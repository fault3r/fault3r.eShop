
using System;

namespace UserService.Domain.Abstractions;

public abstract class ValueObject<TType> : IEquatable<TType>
    where TType : ValueObject<TType>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
        => obj is TType other && Equals(other);

    public bool Equals(TType? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null) return false;
        if (GetType() != other.GetType()) return false;

        return GetEqualityComponents().SequenceEqual(
            other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var component in GetEqualityComponents())
            hash.Add(component);
        return hash.ToHashCode();
    }

    public static bool operator ==(ValueObject<TType>? left, ValueObject<TType>? right)
        => left?.Equals(right) ?? right is null;

    public static bool operator !=(ValueObject<TType>? left, ValueObject<TType>? right)
        => !(left == right);
}
