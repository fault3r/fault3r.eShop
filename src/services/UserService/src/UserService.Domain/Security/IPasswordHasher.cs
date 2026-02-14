
using System;

namespace UserService.Domain.Security;

public interface IPasswordHasher
{
    string Compute(string raw);
    bool Verify(string raw, string hash);
    string DummyHash { get; }

    string GenerateSalt();
    string DummySalt { get; }
}
