
using System;

namespace UserService.Domain.Exceptions.Role;

public class RoleException(
    string message
) : DomainException(message) { }