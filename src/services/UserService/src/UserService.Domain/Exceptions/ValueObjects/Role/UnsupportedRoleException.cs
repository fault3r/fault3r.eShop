
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Role;

public class UnsupportedRoleException : RoleException
{
    public UnsupportedRoleException(string value)
        : base($"unsupported role: {value}"){}
}
