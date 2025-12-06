
using System;
using UserService.Domain.Common;
using UserService.Domain.Interfaces;
using UserService.Domain.Repositories;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Services;

public class UserDomainService : IUserDomainService
{
    private readonly IUserRepository _userRepository;

    public UserDomainService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> CanCreateUserAsync(Email email)
    {
        var exists = await _userRepository.GetByEmailAsync(email);
        return exists is not null
            ? Result.Failure("A user with this email already exists!")
            : Result.Success();
    }
}