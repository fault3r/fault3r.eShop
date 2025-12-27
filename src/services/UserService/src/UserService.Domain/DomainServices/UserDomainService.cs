
using System;
using UserService.Domain.Exceptions.Repositories;
using UserService.Domain.Exceptions.ValueObjects.Email;
using UserService.Domain.Interfaces;
using UserService.Domain.Repositories;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.DomainServices;

public class UserDomainService : IUserDomainService
{
    private readonly IUserRepository _userRepository;

    public UserDomainService(IUserRepository userRepository)
    {
        _userRepository = userRepository
            ?? throw new MissingUserRepositoryException();
    }

    public async Task<bool> CanCreateAsync(
        Email email,
        CancellationToken cancellationToken = default)
    {
        if (email is null)
            throw new MissingEmailException();

        var exists = await _userRepository
            .GetByEmailAsync(email, cancellationToken);

        return exists is null;
    }
}