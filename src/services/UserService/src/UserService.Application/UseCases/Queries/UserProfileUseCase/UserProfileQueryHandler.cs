
using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.Queries.UserProfileUseCase;

public sealed class UserProfileQueryHandler(
    IUserProfileService userProfileService,
    IValidator<UserProfileQuery> validator,
    ILogger<UserProfileQueryHandler> logger
) : IRequestHandler<UserProfileQuery, Result<UserProfileResult>>
{
    private readonly IUserProfileService _profileService = userProfileService;
    private readonly IValidator<UserProfileQuery> _validator = validator;
    private readonly ILogger<UserProfileQueryHandler> _logger = logger;

    public async Task<Result<UserProfileResult>> Handle(
        UserProfileQuery request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errors = string.Join(" - ", validation.Errors.Select(e => e.ErrorMessage));

            _logger.LogWarning("Validation failed: {Error}", errors);

            return Result<UserProfileResult>.Failure(errors);
        }

        var result = await _profileService.ExecuteAsync(
            sessionId: request.SessionId,
            cancellationToken: cancellationToken
        );

        return result;
    }
}
