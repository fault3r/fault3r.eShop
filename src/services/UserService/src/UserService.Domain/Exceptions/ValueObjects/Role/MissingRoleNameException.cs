
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Role;

public sealed class MissingRoleNameException : DomainException
{
    public MissingRoleNameException() : base("Role name is required"){}
}
