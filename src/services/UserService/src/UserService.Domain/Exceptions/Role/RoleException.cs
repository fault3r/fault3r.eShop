
using System;

namespace UserService.Domain.Exceptions.Role;

public class RoleException : DomainException
{
    public RoleException(string message)
        : base(message) { }
}
