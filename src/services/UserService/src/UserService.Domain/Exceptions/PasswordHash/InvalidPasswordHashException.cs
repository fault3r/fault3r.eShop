
using System;

namespace UserService.Domain.Exceptions.PasswordHash;

public sealed class InvalidPasswordHashException(
    string value
) : PasswordHashException($"invalid password hash length: {value}") { }
