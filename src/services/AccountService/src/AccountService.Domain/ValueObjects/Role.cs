
using System;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Exceptions;

namespace AccountService.Domain.ValueObjects;

public sealed class Role : ValueObject
{
    public string Name { get; private set; }

    private Role(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new MissingRoleException();
        var normalized = name.Trim();
        Name = normalized switch
        {
            "admin" or "Admin" or "ADMIN" => "Admin",
            "user" or "User" or "USER" => "User",
            _ => throw new UnsupportedRoleException(normalized)
        };
    }

    public static readonly Role Admin = new(nameof(Admin));
    public static readonly Role User = new(nameof(User));

    public static Role From(string name)
        => new(name);

    public override string ToString()
        => Name;
        
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}