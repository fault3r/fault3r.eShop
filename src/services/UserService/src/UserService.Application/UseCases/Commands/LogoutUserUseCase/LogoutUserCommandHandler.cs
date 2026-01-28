
using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Contracts;

namespace UserService.Application.UseCases.Commands.LogoutUserUseCase;

public sealed class LogoutUserCommandHandler(
    ILogoutUserService logoutUserService,
    IValidator<LogoutUserCommand> validator,
    ILogger<LogoutUserCommandHandler> logger
) : IRequestHandler<LogoutUserCommand, Result>
{

    private readonly ILogoutUserService _logoutService = logoutUserService;
    private readonly IValidator<LogoutUserCommand> _validator = validator;
    private readonly ILogger<LogoutUserCommandHandler> _logger = logger;

    public async Task<Result> Handle(
        LogoutUserCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errors = string.Join(" - ", validation.Errors.Select(e => e.ErrorMessage));

            _logger.LogWarning("Validation failed: {Error}", errors);

            return Result.Failure(errors);
        }

        var result = await _logoutService.ExecuteAsync(
            sessionId: request.SessionId,
            cancellationToken: cancellationToken
        );

        return result;
    }
}