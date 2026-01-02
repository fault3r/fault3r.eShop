
using System;
using FluentValidation;

namespace UserService.Application.UseCases.UserAggregate.SignInUser;

public sealed class SignInUserValidator : AbstractValidator<SignInUserCommand>
{
    public SignInUserValidator()
    {
        RuleFor(p => p.Identity)
            .NotEmpty();

        RuleFor(p => p.Password)
            .NotEmpty();
    }
}
