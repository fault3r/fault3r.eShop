
using System;

namespace UserService.Domain.Exceptions.PasswordHash;

public class PasswordHashException(
    string message
) : DomainException(message) { }