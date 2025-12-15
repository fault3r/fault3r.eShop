
using System;
using UserService.Application.Interfaces;
using UserService.Application.Security;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;
using UserService.Domain.Exceptions;
using UserService.Domain.Factories;
using UserService.Domain.Interfaces;
using UserService.Domain.UnitOfWork;
using UserService.Domain.ValueObjects;

namespace UserService.Application.UseCases.SignUpUser;

public sealed class SignUpUserService : ISignUpUserService
{
    private readonly IUnitOfWork _uow;
    private readonly IUserDomainService _domainService;

    private readonly IPasswordHasher _passwordHasher;

    public SignUpUserService(
        IUnitOfWork uow,
        IUserDomainService domainService,
        IPasswordHasher passwordHasher)
    {
        _uow = uow;
        _domainService = domainService;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<User>> ExecuteAsync(
        string email,
        string password,
        string fullName,
        string correlationId,
        CancellationToken cancellationToken = default)
    {
        User user;
        Email vEmail;
        PasswordHash vPasswordHash;
        FullName vFullName;
        try
        {
            vEmail = Email.Parse(email);

            string hashed = _passwordHasher.Hash(password);
            vPasswordHash = PasswordHash.Parse(hashed);

            vFullName = FullName.Parse(fullName);

            user = UserFactory.CreateNew(vEmail, vPasswordHash, vFullName);
        }
        catch (DomainException ex)
        {
            return Result<User>.Failure($"Error signing up user: {ex.Message}");
        }
        
        var canCreate = await _domainService.CanCreateUserAsync(vEmail, cancellationToken);
        if (!canCreate)
            return Result<User>.Failure("User with this email already exists!");

        try
        {
            await _uow.Users.CreateAsync(user, cancellationToken);
            await _uow.Outbox.EnqueueAsync(user.DomainEvents, correlationId, cancellationToken);
            await _uow.CommitAsync(cancellationToken);
            user.ClearEvents();
        }
        catch
        {
            return Result<User>.Failure($"An unexpected error occurred!");
        }

        return Result<User>.Success(user);
    }
}
