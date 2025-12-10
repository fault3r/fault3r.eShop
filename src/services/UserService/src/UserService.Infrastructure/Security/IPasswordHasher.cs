
using System;
using UserService.Domain.ValueObjects;

namespace UserService.Infrastructure.Security;

public interface IPasswordHasher
{
    PasswordHash Hash(string rawPassword);
    bool Verify(string rawPassword, PasswordHash passwordHash);
}
