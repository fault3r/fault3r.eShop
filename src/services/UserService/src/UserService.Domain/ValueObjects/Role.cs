
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Common;
using UserService.Domain.Exceptions.Role;

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

    public static readonly Role User = new(RoleType.User);
    public static readonly Role Admin = new(RoleType.Admin);

    public static Role From(RoleType value)
        => new(value);

    public static Role From(string value)
        => new(value);

    public static Result<Role> TryFrom(string value, out Role? role)
    {
        try
        {
            role = From(value);
            return Result<Role>.Success(role);
        }
        catch (RoleException ex)
        {
            role = null;
            return Result<Role>.Failure(ex.Message);
        }
    }

    public bool IsUser => Value == RoleType.User;
    public bool IsAdmin => Value == RoleType.Admin;

    public override string ToString()
        => Value.ToString().ToLowerInvariant();

    public static implicit operator string(Role role)
        => role.ToString();

    public static explicit operator Role(string value)
        => From(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
