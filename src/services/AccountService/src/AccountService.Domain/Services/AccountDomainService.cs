
using System;
using AccountService.Domain.Aggregates.Account;
using AccountService.Domain.Common;
using AccountService.Domain.Exceptions;
using AccountService.Domain.Factories;
using AccountService.Domain.Repositories;

namespace AccountService.Domain.Services;

public class AccountDomainService
{
    private readonly IRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly IOutbox _outbox;

    public AccountDomainService(
        IRepository repository, IUnitOfWork uow, IOutbox outbox)
    {
        _repository = repository
            ?? throw new DomainException("Repository is required");
        _uow = uow
            ?? throw new DomainException("UnitOfWork is required");
        _outbox = outbox
            ?? throw new DomainException("Outbox is required");
    }

    public async Task<Result<Account>> SignUpAsync(
        string fullName, string emailAddress, string passwordHash, CancellationToken cancellationToken = default)
    {
        try
        {
            Account created = AccountFactory
                .CreateNew(fullName, emailAddress, passwordHash);

            await _repository.CreateAsync(created, cancellationToken);
            await _outbox.EnqueueAsync(created.DomainEvents, cancellationToken);
            created.ClearEvents();

            bool result = await _uow.CommitAsync(cancellationToken);

            return result
                ? Result<Account>.Success(created)
                : Result<Account>.Failure("Account sign-up could not be completed.");
        }
        catch (Exception ex)
        {
            return Result<Account>.Failure(
                $"sign-up failed due to an exception! {ex.Message}");
        }
    }
}
