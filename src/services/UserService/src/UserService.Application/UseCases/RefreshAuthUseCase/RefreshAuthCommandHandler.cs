
using System;
using FluentValidation;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.RefreshAuthUseCase;

public sealed class RefreshAuthCommandHandler(
    IRefreshAuthService refreshAuthService,
     IValidator<RefreshAuthCommand> validator)
        : IRequestHandler<RefreshAuthCommand, Result<RefreshAuthResult>>
{
    private readonly IRefreshAuthService _refreshAuthService = refreshAuthService;
    private readonly IValidator<RefreshAuthCommand> _validator = validator;

    public async Task<Result<RefreshAuthResult>> Handle(
        RefreshAuthCommand request,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var validation = await _validator.ValidateAsync(request, ct);

        if (!validation.IsValid)
        {
            var errors = string.Join(" - ", validation.Errors.Select(e => e.ErrorMessage));

            return Result<RefreshAuthResult>.Failure($"Validation failed: {errors}!");
        }

        var result = await _refreshAuthService.ExecuteAsync(
            request.AccessToken,
            request.RefreshToken,
            ct
        );

        return result;
    }

}
