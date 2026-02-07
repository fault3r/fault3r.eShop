
using System;

namespace UserService.Domain.Exceptions.PasswordSalt;

public sealed class InvalidPasswordSaltException(
    string value
) : PasswordSaltException($"invalid password salt length: {value}") { }
