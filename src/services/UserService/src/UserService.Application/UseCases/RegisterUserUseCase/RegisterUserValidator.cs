
using System;
using FluentValidation;

namespace UserService.Application.UseCases.RegisterUserUseCase;

public sealed class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        RuleFor(p => p.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(p => p.Password)
            .NotEmpty()
            .Length(6, 100);

        RuleFor(p => p.FullName)
            .NotEmpty()
            .Length(2, 100);
    }
}
