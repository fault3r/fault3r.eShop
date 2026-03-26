
using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.Commands.RefreshAuthUseCase;

public sealed class RefreshAuthCommandHandler(
    IRefreshAuthService refreshAuthService,
    IValidator<RefreshAuthCommand> validator,
    ILogger<RefreshAuthCommandHandler> logger
) : IRequestHandler<RefreshAuthCommand, Result<RefreshAuthResult>>
{
    private readonly IRefreshAuthService _authService = refreshAuthService;
    private readonly IValidator<RefreshAuthCommand> _validator = validator;
    private readonly ILogger<RefreshAuthCommandHandler> _logger = logger;

    public async Task<Result<RefreshAuthResult>> Handle(
        RefreshAuthCommand request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errors = string.Join(" - ", validation.Errors.Select(e => e.ErrorMessage));

            _logger.LogWarning("Validation failed: {Error}", errors);

            return Result<RefreshAuthResult>.Failure(errors);
        }

        var result = await _authService.ExecuteAsync(
            accessToken: request.AccessToken,
            refreshToken: request.RefreshToken,
            cancellationToken: cancellationToken
        );

        return result;
    }
}
