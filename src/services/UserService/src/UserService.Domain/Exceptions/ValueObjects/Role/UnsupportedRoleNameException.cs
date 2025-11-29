
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Role;

public sealed class UnsupportedRoleNameException : DomainException
{
    public UnsupportedRoleNameException(string roleName) : base($"unsupported role name: {roleName}"){}
}
