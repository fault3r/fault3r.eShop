
using System;
using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using UserService.Domain.Contracts;
using UserService.Domain.Security;

namespace UserService.Infrastructure.Security;

public sealed class Argon2PasswordHasher : IPasswordHasher
{
    public string Compute(string raw)
    {
        var salt = RandomNumberGenerator.GetBytes(16);

        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(raw))
        {
            Salt = salt,
            DegreeOfParallelism = 4,
            Iterations = 3,
            MemorySize = 65536
        };

        var hashBytes = argon2.GetBytes(32);

        var hashString =
            $"$argon2id$v=19$m=65536,t=3,p=4${Convert.ToBase64String(salt)}${Convert.ToBase64String(hashBytes)}";

        return hashString;
    }

    public bool Verify(string raw, string hash)
    {
        var parts = hash.Split('$', StringSplitOptions.RemoveEmptyEntries);

        var parameters = parts[2].Split(',');
        var memory = int.Parse(parameters[0].Split('=')[1]);
        var iterations = int.Parse(parameters[1].Split('=')[1]);
        var parallelism = int.Parse(parameters[2].Split('=')[1]);

        var salt = Convert.FromBase64String(parts[3]);
        var expectedHash = Convert.FromBase64String(parts[4]);

        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(raw))
        {
            Salt = salt,
            DegreeOfParallelism = parallelism,
            Iterations = iterations,
            MemorySize = memory
        };

        var computedHash = argon2.GetBytes(expectedHash.Length);

        return CryptographicOperations.FixedTimeEquals(computedHash, expectedHash);
    }

    public string DummyHash
        => "$argon2id$v=19$m=65536,t=3,p=4$AAAAAAAAAAAAAAAAAAAAAA"
            + "==$AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";

    public string GenerateSalt()
        => RandomStringGenerator.GetString(length: 4);

    public string DummySalt
        => "salt";
}