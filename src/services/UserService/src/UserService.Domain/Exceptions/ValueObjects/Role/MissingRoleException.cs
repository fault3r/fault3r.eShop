
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Role;

public class MissingRoleException : RoleException
{
    public MissingRoleException()
        : base("role is required"){}
}
