
using System;

namespace UserService.Domain.Abstractions;

public abstract class ValueObject<T> : IEquatable<T>
    where T : ValueObject<T>
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
        => obj is T other && Equals(other);

    public bool Equals(T? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null) return false;

        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        foreach (var component in GetEqualityComponents())
            hash.Add(component);

        return hash.ToHashCode();
    }

    public static bool operator ==(ValueObject<T>? left, ValueObject<T>? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;

        return left.Equals((T)right);
    }

    public static bool operator !=(ValueObject<T>? left, ValueObject<T>? right)
        => !(left == right);
}
