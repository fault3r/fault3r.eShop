
using System;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Exceptions.Role;

namespace AccountService.Domain.ValueObjects;

public sealed class Role : ValueObject
{
    public RoleType Name { get; }

    public enum RoleType
    {
        User,
        Admin,
    }

    public Role(RoleType name)
        => Name = name;

    public Role(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new MissingRoleException();

        var normalized = name.Trim().ToLowerInvariant();
        Name = normalized switch
        {
            "user" => RoleType.User,
            "admin" => RoleType.Admin,
            _ => throw new UnsupportedRoleException(normalized),
        };
    }

    public static readonly Role User = new(RoleType.User);
    public static readonly Role Admin = new(RoleType.Admin);

    public static Role From(string input)
        => new(input);

    public bool IsUser
        => Name == RoleType.User;

    public bool IsAdmin
        => Name == RoleType.Admin;

    public override string ToString()
        => Name.ToString();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}