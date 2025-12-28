
using System;

namespace UserService.Domain.Exceptions.Role;

public sealed class UnsupportedRoleException : RoleException
{
    public UnsupportedRoleException(string value)
        : base($"unsupported role: {value}"){}
}
