
using System;

namespace UserService.Domain.Exceptions.Identity;

public class IdentityException(
    string message
) : DomainException(message) { }
