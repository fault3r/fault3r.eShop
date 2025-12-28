
using System;

namespace UserService.Domain.Exceptions.Role;

public sealed class MissingRoleException : RoleException
{
    public MissingRoleException()
        : base("role is required"){}
}
