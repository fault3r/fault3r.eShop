
using System;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Security;

public interface IPasswordHasher
{
    PasswordHash Hash(string rawPassword);
    bool Verify(string rawPassword, PasswordHash passwordHash);
}
