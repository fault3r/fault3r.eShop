
using System;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Interfaces;
using UserService.Domain.Repositories;
using UserService.Domain.Security;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.DomainServices;

public class UserDomainService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher) : IUserDomainService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<bool> VerifyCanCreateAsync(
        Email email,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(email);

        var exists = await _userRepository
            .GetByEmailAsync(email, cancellationToken);

        return exists is null;
    }

    public async Task<User?> VerifyCredentialsAsync(
       string identity,
       string password,
       CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identity);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        if (!Email.TryFrom(identity, out var email))
            return null;

        var user = await _userRepository!.GetByEmailAsync(email!, cancellationToken);

        // ⟶timing attack!
        string hash = user is null ? _passwordHasher.DummyHash : user.PasswordHash;

        bool verified = _passwordHasher.Verify(password, hash);

        return verified ? user : null;
    }
}