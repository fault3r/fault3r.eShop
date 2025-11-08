using System;

namespace AccountService.Domain.Exceptions.Role;

public class UnsupportedRoleException : DomainException
{
    public UnsupportedRoleException(string name) : base($"unsupported Role: {name}") {}
}