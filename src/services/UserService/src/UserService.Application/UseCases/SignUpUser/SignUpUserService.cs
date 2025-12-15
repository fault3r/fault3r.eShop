
using System;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<SignUpUserService> _logger;

    public SignUpUserService(
        IUnitOfWork uow,
        IUserDomainService domainService,
        IPasswordHasher passwordHasher,
        ILogger<SignUpUserService> logger)
    {
        _uow = uow;
        _domainService = domainService;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<Result<User>> ExecuteAsync(
        string email,
        string password,
        string fullName,
        string correlationId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Executing service");
            
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

            _logger.LogInformation("User created successfully");
        }
        catch (DomainException exception)
        {
            _logger.LogWarning(exception, "Domain validation failed");

            return Result<User>.Failure($"Cannot create user: {exception.Message}");
        }
        
        _logger.LogInformation("Checking if user can be created");
        
        var canCreate = await _domainService.CanCreateUserAsync(vEmail, cancellationToken);
        if (!canCreate)
        {
            _logger.LogWarning("User with this email already exists");
            
            return Result<User>.Failure("User with this email already exists!");
        }

        try
        {
            _logger.LogInformation("Persisting user to database");

            await _uow.Users.CreateAsync(user, cancellationToken);
            await _uow.Outbox.EnqueueAsync(user.DomainEvents, correlationId, cancellationToken);
            await _uow.CommitAsync(cancellationToken);
            user.ClearEvents();

            _logger.LogInformation("User successfully created and committed");
        }
        catch(Exception exception)
        {
            _logger.LogError(exception, "Unexpected error while committing changes");
            
            return Result<User>.Failure($"An unexpected error occurred!");
        }

        return Result<User>.Success(user);
    }
}
                