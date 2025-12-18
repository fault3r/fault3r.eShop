
using System;
using UserService.Domain.Exceptions.Repositories;
using UserService.Domain.Repositories;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Services.UserService;

public class UserDomainService : IUserDomainService
{
    private readonly IUserRepository _userRepository;

    public UserDomainService(IUserRepository userRepository)
    {
        _userRepository = userRepository
            ?? throw new MissingUserRepositoryException();
    }

    public async Task<bool> CanCreateUserAsync(
        Email email, CancellationToken cancellationToken = default)
    {
        var exists = await _userRepository.GetByEmailAsync(email, cancellationToken);
        return exists is null;
    }
}