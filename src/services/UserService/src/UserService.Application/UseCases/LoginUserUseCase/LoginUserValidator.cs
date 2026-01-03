
using System;
using FluentValidation;

namespace UserService.Application.UseCases.LoginUserUseCase;

public sealed class LoginUserValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserValidator()
    {
        RuleFor(p => p.Identity)
            .EmailAddress()
            .NotEmpty();

        RuleFor(p => p.Password)
            .Length(8,100)
            .NotEmpty();
    }
}
