
using System;
using FluentValidation;

namespace UserService.Application.UseCases.Queries.UserProfileUseCase;

public sealed class UserProfileValidator : AbstractValidator<UserProfileQuery>
{
    public UserProfileValidator()
    {
        RuleFor(p => p.SessionId)
            .NotEmpty();
    }
}
