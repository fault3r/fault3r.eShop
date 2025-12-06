
using System;
using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using UserService.Domain.Security;
using UserService.Domain.ValueObjects;

namespace UserService.Infrastructure.Security;

public sealed class Argon2PasswordHasher : IPasswordHasher
{
    public PasswordHash Hash(string rawPassword)
    {
        var salt = RandomNumberGenerator.GetBytes(16);

        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(rawPassword))
        {
            Salt = salt,
            DegreeOfParallelism = 4,
            Iterations = 3,
            MemorySize = 65536
        };

        var hashBytes = argon2.GetBytes(32);

        var hashString =
            $"$argon2id$v=19$m=65536,t=3,p=4${Convert.ToBase64String(salt)}${Convert.ToBase64String(hashBytes)}";

        return PasswordHash.Parse(hashString);
    }

    public bool Verify(string rawPassword, PasswordHash hash)
    {
        var parts = hash.Value.Split('$', StringSplitOptions.RemoveEmptyEntries);

        var parameters = parts[2].Split(',');
        var memory = int.Parse(parameters[0].Split('=')[1]);
        var iterations = int.Parse(parameters[1].Split('=')[1]);
        var parallelism = int.Parse(parameters[2].Split('=')[1]);

        var salt = Convert.FromBase64String(parts[3]);
        var expectedHash = Convert.FromBase64String(parts[4]);

        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(rawPassword))
        {
            Salt = salt,
            DegreeOfParallelism = parallelism,
            Iterations = iterations,
            MemorySize = memory
        };

        var computedHash = argon2.GetBytes(expectedHash.Length);

        return CryptographicOperations.FixedTimeEquals(computedHash, expectedHash);
    }
}