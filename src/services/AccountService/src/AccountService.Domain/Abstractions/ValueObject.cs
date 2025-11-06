
using System;

namespace AccountService.Domain.Abstractions;

public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is not ValueObject other) return false;
        if (ReferenceEquals(this, other)) return true;
        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach(var obj in GetEqualityComponents())
            hash.Add(obj.GetHashCode());
        return hash.ToHashCode();
    }
}
