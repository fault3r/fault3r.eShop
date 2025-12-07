
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Role;

public sealed class MissingRoleException : DomainException
{
    public MissingRoleException()
        : base("role is required"){}
}
