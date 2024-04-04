

using System;
using System.Security.Cryptography;
using System.Text;

namespace fault3r_Common
{
    public class PasswordHasher
    {
        public static string ToHash(string password)
        {
            var hasher = SHA256.Create();
            byte[] hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);             
        }

    }
}
