
using System;
using AccountService.Domain.Exceptions;

namespace AccountService.Domain.Accounts;

public sealed class Role : IEquatable<Role>
{
    public string Name { get; private set; }

    private Role(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Role is required");
        var normalized = name.Trim();
        Name = normalized switch
        {
            "admin" or "Admin" or "ADMIN" => "Admin",
            "user" or "User" or "USER" => "User",
            _ => throw new DomainException($"Invalid role: {normalized}")
        };
    }

    public static readonly Role Admin = new("Admin");
    public static readonly Role User = new("User");

    public static Role From(string name)
        => new(name);

    public bool Equals(Role? other)
        => other is not null && other.Name == Name;

    public override bool Equals(object? obj)
        => obj is Role && Equals(obj as Role);

    public override int GetHashCode()
        => Name.GetHashCode(StringComparison.Ordinal);

    public static bool operator ==(Role? left, Role? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Role? left, Role? right)
        => !(left == right);

    public static explicit operator Role(string name)
        => new(name);

    public static implicit operator string(Role role)
        => role.Name;
}
