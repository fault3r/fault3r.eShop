
using System;
using UserService.Domain.ValueObjects;

namespace UserService.Application.Security;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}
