
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
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(email);

        var exists = await _userRepository
            .GetByEmailAsync(email, ct);

        return exists is null;
    }

    public async Task<User?> VerifyCredentialAsync(
       string identity,
       string password,
       CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identity);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        var email = Email.From(identity);
        var user = await _userRepository.GetByEmailAsync(email, ct);

        string hash = user?.PasswordHash ?? _passwordHasher.DummyHash;
        bool verified = _passwordHasher.Verify(password, hash);

        return verified ? user : null;
    }
}