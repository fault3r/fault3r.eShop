
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.Role;
using static UserService.Domain.ValueObjects.Role;

namespace UserService.Domain.ValueObjects;

public sealed record Role : ValueObject<RoleType>
{
    public override RoleType Value { get; }

    public enum RoleType
    {
        User = 1,
        Admin = 101,
    }

    public Role(RoleType roleType)
        => Value = roleType;

    public Role(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            throw new MissingRoleNameException();

        var normalized = roleName
            .Trim()
            .ToLowerInvariant();
        Value = normalized switch
        {
            "user" => RoleType.User,
            "admin" => RoleType.Admin,
            _ => throw new UnsupportedRoleNameException(normalized),
        };
    }

    public static readonly Role User = new(RoleType.User);
    public static readonly Role Admin = new(RoleType.Admin);

    public static Role From(RoleType roleType)
        => new(roleType);

    public static Role Parse(string roleName)
        => new(roleName);

    public override string ToString()
        => Value.ToString();

    public bool IsUser
        => Value == RoleType.User;

    public bool IsAdmin
        => Value == RoleType.Admin;
}