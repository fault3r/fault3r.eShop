
using System;

namespace UserService.Domain.Exceptions.PasswordSalt;

public sealed class MissingPasswordSaltException : PasswordSaltException
{
    public MissingPasswordSaltException()
        : base($"password salt is required") { }
}
