
using System;
using AccountService.Application.Interfaces.Common;

namespace AccountService.Application.Common
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password) =>
            BCrypt.Net.BCrypt.HashPassword(password);

        public bool Verify(string password, string hashed) =>
            BCrypt.Net.BCrypt.Verify(password, hashed);
    }
}