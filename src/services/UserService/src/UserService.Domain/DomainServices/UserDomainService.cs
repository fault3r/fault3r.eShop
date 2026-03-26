
using System;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;
using UserService.Domain.Interfaces;
using UserService.Domain.Repositories;
using UserService.Domain.Security;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.DomainServices;

public class UserDomainService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher
) : IUserDomainService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<bool> VerifyCanCreateAsync(
        Email email,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(email);

        var exists = await _userRepository.GetByEmailAsync(email, cancellationToken);

        return exists is null;
    }

    public async Task<Result<User>> VerifyCredentialsAsync(
       string identity,
       string password,
       CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identity);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        if (!Email.TryParse(identity, out var email))
            return Result<User>.Failure("Invalid identity!");

        var user = await _userRepository.GetByEmailAsync(email!, cancellationToken);

        // ⟶rainbow table attack!
        string raw = password +
            (user is null
                ? _passwordHasher.DummySalt
                : user.PasswordSalt
            );

        // ⟶timing attack!
        string hash = user is null
            ? _passwordHasher.DummyHash
            : user.PasswordHash;

        bool verify = _passwordHasher.Verify(raw, hash);

        if (!verify)
            return Result<User>.Failure("Invalid credentials!");

        return Result<User>.Success(user!);
    }
}