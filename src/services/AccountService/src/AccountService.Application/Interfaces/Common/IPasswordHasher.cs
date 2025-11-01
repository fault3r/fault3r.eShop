
using System;

namespace AccountService.Application.Interfaces.Common
{
    public interface IPasswordHasher
    {
        string Hash(string password);

        bool Verify(string password, string hashed);
    }
}
