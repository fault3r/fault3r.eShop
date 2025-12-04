
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.Role;
using static UserService.Domain.ValueObjects.Role;

namespace UserService.Domain.ValueObjects;

public sealed record Role : ValueObject<RoleType>
{
    public override RoleType Value { get; init; }

    public enum RoleType
    {
        User = 1,
        Admin = 101,
    }

    private Role(RoleType roleType)
        => Value = roleType;

    private Role(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new MissingRoleNameException();

        var normalized = Normalize(value);

        Value = normalized switch
        {
            "user" => RoleType.User,
            "admin" => RoleType.Admin,
            _ => throw new UnsupportedRoleNameException(normalized),
        };
    }

    private static string Normalize(string value)
        => value.Trim().ToLowerInvariant();

    public static Role From(RoleType value)
        => new(value);

    public static Role Parse(string value)
        => new(value);

    public static bool TryParse(string value, out Role? role)
    {
        try
        {
            role = new(value);
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
    
    public static IEnumerable<Role> All =>  [User, Admin];

    public bool IsUser
        => Value == RoleType.User;
    public bool IsAdmin
        => Value == RoleType.Admin;

    public override string ToString()
        => Value.ToString();

    public static implicit operator string(Role role)
        => role.Value.ToString();

    public static explicit operator Role(string value)
        => Parse(value);
}
