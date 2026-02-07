
using System;

namespace UserService.Domain.Exceptions.PasswordSalt;

public class PasswordSaltException(
    string message
) : DomainException(message) { }