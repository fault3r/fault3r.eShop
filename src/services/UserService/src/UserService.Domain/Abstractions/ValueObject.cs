
using System;

namespace UserService.Domain.Abstractions;

public abstract class ValueObject<T> : IEquatable<T>
    where T : ValueObject<T>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
        => obj is T other && Equals(other);

    public bool Equals(T? other)
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

    public static bool operator ==(ValueObject<T>? left, ValueObject<T>? right)
        => Equals(left, right);

    public static bool operator !=(ValueObject<T>? left, ValueObject<T>? right)
        => !Equals(left, right);
}
