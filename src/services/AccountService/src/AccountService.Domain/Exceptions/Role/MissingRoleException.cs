using System;

namespace AccountService.Domain.Exceptions.Role;

public class MissingRoleException : DomainException
{
    public MissingRoleException() : base($"Role is required"){}
}
