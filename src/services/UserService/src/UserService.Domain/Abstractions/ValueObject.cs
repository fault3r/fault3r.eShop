using System;

namespace UserService.Domain.Abstractions;

public abstract class ValueObject<T> : IEquatable<T>
    where T : ValueObject<T>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
        => obj is T other 
        && Equals(other as T);

    public bool Equals(T? other)
        => other is not null
        && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

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
