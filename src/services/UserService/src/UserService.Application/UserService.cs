/*
using System;
using UserService.Domain.Aggregates;
using UserService.Domain.Common;
using UserService.Domain.Factories;
using UserService.Domain.UnitOfWork;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Services;

public class UserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<User>> SignUpAsync(
       Email email,
       PasswordHash passwordHash,
       FullName fullName,
       CancellationToken cancellationToken = default)
    {
        try
        {
            var user = UserFactory.CreateNew(email, passwordHash, fullName);

            await _unitOfWork.Users.CreateAsync(user, cancellationToken);
            await _unitOfWork.Outbox.EnqueueAsync(user.DomainEvents, cancellationToken);
            await _unitOfWork.CommitChangesAsync(cancellationToken);

            return Result<User>.Success(user);
        }
        catch (Exception ex)
        {
            return Result<User>.Failure(
                $"sign-up failed due to an exception: {ex.Message}");
        }
    }
}
*/