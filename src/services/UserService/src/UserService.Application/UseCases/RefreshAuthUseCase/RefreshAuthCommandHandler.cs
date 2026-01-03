
using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.RefreshAuthUseCase;

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
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var validation = await _validator.ValidateAsync(request, ct);

        if (!validation.IsValid)
        {
            var errors = string.Join(" - ", validation.Errors.Select(e => e.ErrorMessage));

            _logger.LogWarning("Validation failed: {Error}", errors);

            return Result<RefreshAuthResult>.Failure(errors);
        }

        var result = await _authService.ExecuteAsync(
            expiredAccessToken: request.AccessToken,
            providedRefreshToken: request.RefreshToken,
            ct: ct
        );

        return result;
    }
}
