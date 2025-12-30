
using System;
using UserService.Application.Interfaces;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;
using UserService.Domain.Exceptions;
using UserService.Domain.Factories;
using UserService.Domain.UnitOfWork;
using UserService.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using UserService.Domain.Interfaces;
using UserService.Application.Security;
using UserService.Application.CrossCutting;

namespace UserService.Application.UseCases.UserAggregate.SignUpUser;

public sealed class SignUpUserService(
    IUnitOfWork unitOfWork,
    IUserDomainService userDomainService,
    ICorrelationContext correlation,
    IPasswordHasher passwordHasher,
    ILogger<SignUpUserService> logger)
        : ISignUpUserService
{
    private readonly IUnitOfWork _uow = unitOfWork;
    private readonly IUserDomainService _userService = userDomainService;
    private readonly ICorrelationContext _correlation = correlation;
    private readonly IPasswordHasher _hasher = passwordHasher;
    private readonly ILogger<SignUpUserService> _logger = logger;

    public async Task<Result<User>> ExecuteAsync(
        string email,
        string password,
        string fullName,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(password);
        ArgumentNullException.ThrowIfNull(fullName);

        User user;
        Email vEmail;
        PasswordHash vPasswordHash;
        FullName vFullName;
        try
        {
            _logger.LogInformation("Creating user with '{Email}' email address..", email.Trim());
            
            vEmail = Email.From(email);

            string hashed = _hasher.Hash(password);
            vPasswordHash = PasswordHash.From(hashed);

            vFullName = FullName.From(fullName);

            user = UserFactory.Create(vEmail, vPasswordHash, vFullName);

            _logger.LogInformation("User instance created successfully.");
        }
        catch (DomainException ex)
        {
            _logger.LogWarning("Domain validation failed: {Error}!", ex.Message);

            return Result<User>.Failure($"Domain validation failed: {ex.Message}!");
        }

        _logger.LogInformation("Checking whether the user can be created…");

        var canCreate = await _userService.CanCreateAsync(vEmail, cancellationToken);

        if (!canCreate)
        {
            _logger.LogWarning("User with this email already exists!");

            return Result<User>.Failure("Sign up failed: User with this email already exists!");
        }

        _logger.LogInformation("The user is allowed to create.");

        _logger.LogInformation("Persisting user data to the database…");

        await _uow.UserRepository.CreateAsync(user, cancellationToken);
        await _uow.Outbox.EnqueueAsync(user.Events, _correlation.CorrelationId, cancellationToken);
        await _uow.CommitAsync(cancellationToken);

        _logger.LogInformation("User data successfully persisted.");
        
        _logger.LogInformation("Dispatching user creation notification…");

        await _uow.Notification.DispatchAsync(user.Events, cancellationToken);

        _logger.LogInformation("Notification dispatched successfully.");

        user.ClearEvents();

        _logger.LogInformation("User successfully created with '{Id}' identity.", user.Id.ToString());

        return Result<User>.Success(user);
    }
}
