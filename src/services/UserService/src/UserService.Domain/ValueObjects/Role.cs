
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.Role;

namespace UserService.Domain.ValueObjects;

public sealed class Role : ValueObject<Role>
{
    public RoleType Name { get; }

    public enum RoleType
    {
        User = 1,
        Admin = 101,
    }

    public Role(RoleType name)
        => Name = name;

    public Role(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new MissingRoleNameException();

        var normalized = name
            .Trim()
            .ToLowerInvariant();
        Name = normalized switch
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
        => Name.ToString();

    public bool IsUser
        => Name == RoleType.User;

    public bool IsAdmin
        => Name == RoleType.Admin;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
    }
}