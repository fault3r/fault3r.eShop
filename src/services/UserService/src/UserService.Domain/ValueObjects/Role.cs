
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions;
using UserService.Domain.Exceptions.ValueObjects.Role;

namespace UserService.Domain.ValueObjects;

public sealed class Role : ValueObject<Role>
{
    public RoleType Value { get; }

    public enum RoleType
    {
        User = 1,
        Admin = 101,
    }

    private Role(RoleType roleType)
    {
        Value = roleType;
    }

    private Role(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingRoleNameException();

        value = value.Trim().ToLowerInvariant();

        var role = value switch
        {
            "user" => RoleType.User,
            "admin" => RoleType.Admin,
            _ => throw new UnsupportedRoleNameException(value),
        };

        Value = role;
    }

    public static readonly Role User = new(RoleType.User);
    public static readonly Role Admin = new(RoleType.Admin);

    public static Role From(RoleType value)
        => value switch
        {
            RoleType.User => User,
            RoleType.Admin => Admin,
            _ => throw new UnsupportedRoleNameException(value.ToString())
        };

    public static Role Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingRoleNameException();

        value = value.Trim().ToLowerInvariant();

        return value switch
        {
            "user" => User,
            "admin" => Admin,
            _ => throw new UnsupportedRoleNameException(value)
        };
    }

    public static bool TryParse(string value, out Role? role)
    {
        try
        {
            role = Parse(value);
            return true;
        }
        catch (DomainException)
        {
            role = null;
            return false;
        }
    }

    public bool IsUser => Value == RoleType.User;
    public bool IsAdmin => Value == RoleType.Admin;

    public override string ToString()
        => Value.ToString();

    public static implicit operator string(Role role)
        => role.ToString();

    public static explicit operator Role(string value)
        => Parse(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
