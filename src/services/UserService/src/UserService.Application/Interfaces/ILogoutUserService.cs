using System;
using UserService.Domain.Common;

namespace UserService.Application.Interfaces;

public interface ILogoutUserService
{
    Task<Result> ExecuteAsync(
        string sessionId,
        CancellationToken cancellationToken
    );
}
