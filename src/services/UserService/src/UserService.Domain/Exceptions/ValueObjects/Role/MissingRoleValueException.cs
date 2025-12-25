
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Role;

public class MissingRoleValueException : RoleException
{
    public MissingRoleValueException()
        : base("role value is required"){}
}
