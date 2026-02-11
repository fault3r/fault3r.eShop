
using System;

namespace UserService.Domain.Security;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
    
    string DummyHash { get; }

    string GenerateSalt();
}
