
using System;

namespace UserService.Domain.Security;

public interface IPasswordHasher
{
    string Compute(string password);
    bool Verify(string password, string hash);
    
    string DummyHash { get; }

    string GenerateSalt();
}
