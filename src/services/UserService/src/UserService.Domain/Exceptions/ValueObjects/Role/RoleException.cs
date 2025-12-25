
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Role;

public class RoleException : DomainException
{
    public RoleException(string message)
        : base(message) { }
}
