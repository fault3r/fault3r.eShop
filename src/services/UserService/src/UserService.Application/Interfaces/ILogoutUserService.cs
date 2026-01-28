using System;
using UserService.Domain.Contracts;

namespace UserService.Application.Interfaces;

public interface ILogoutUserService
{
    Task<Result> ExecuteAsync(
        string sessionId,
        CancellationToken cancellationToken = default
    );
}
