
using System;
using FluentValidation;

namespace UserService.Application.UseCases.RefreshAuthUseCase;

public sealed class RefreshAuthValidator : AbstractValidator<RefreshAuthCommand>
{
    public RefreshAuthValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty();

        RuleFor(x => x.RefreshToken)
            .NotEmpty();
    }
}
