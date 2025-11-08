
using System;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Exceptions.Role;

namespace AccountService.Domain.ValueObjects;

public sealed class Role : ValueObject
{
    public RoleType Value { get; }

    public enum RoleType
    {
        User,
        Admin,
    }

    public Role(RoleType value)
        => Value = value;

    public Role(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new MissingRoleException();
            
        var normalized = name.Trim().ToLowerInvariant();
        Value = normalized switch
        {
            "user" => RoleType.User,
            "admin" => RoleType.Admin,
            _ => throw new UnsupportedRoleException(normalized)
        };
    }

    public static readonly Role User = new(RoleType.User);
    public static readonly Role Admin = new(RoleType.Admin);

    public static Role From(string input)
        => new(input);

    public bool IsUser
        => Value == RoleType.User;

    public bool IsAdmin
        => Value == RoleType.Admin;

    public override string ToString()
        => Value.ToString();
        
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}