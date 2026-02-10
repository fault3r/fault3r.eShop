
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.Role;

namespace UserService.Domain.ValueObjects;

public sealed class Role : ValueObject<Role>
{
    public enum RoleType
    {
        User = 1,
        Admin = 101,
    }

    public RoleType Value { get; }

    private Role(RoleType role)
    {
        Value = role;
    }

    private Role(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingRoleException();

        value = value
            .Trim()
            .ToLowerInvariant();

        var role = value switch
        {
            "user" => RoleType.User,
            "admin" => RoleType.Admin,
            _ => throw new UnsupportedRoleException(value),
        };

        Value = role;
    }

    public static Role From(RoleType value)
        => new(value);

    public static Role Parse(string value)
        => new(value);

    public static bool TryParse(string value, out Role? role)
    {
        try
        {
            role = Parse(value);
            return true;
        }
        catch
        {
            role = null;
            return false;
        }
    }

    public static readonly Role User = new(RoleType.User);
    public static readonly Role Admin = new(RoleType.Admin);
    
    public bool IsUser => Value == RoleType.User;
    public bool IsAdmin => Value == RoleType.Admin;

    public override string ToString()
        => Value.ToString().ToLower();

    public static implicit operator string(Role role)
        => role.ToString();

    public static explicit operator Role(string value)
        => Parse(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
