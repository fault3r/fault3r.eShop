
using System;
using FluentValidation;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Domain.Aggregates;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.SignUpUser;

public class SignUpUserCommandHandler
    : IRequestHandler<SignUpUserCommand, Result<User>>
{
    private readonly ISignUpUserService _signUpUserService;

    private readonly IValidator<SignUpUserCommand> _validator;

    public SignUpUserCommandHandler(
        ISignUpUserService signUpUserService,
        IValidator<SignUpUserCommand> validator)
    {
        _signUpUserService = signUpUserService;
        _validator = validator;
    }
    
    public async Task<Result<User>> Handle(SignUpUserCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return Result<User>.Failure(
                string.Join("\n", validation.Errors.Select(e => e.ErrorMessage))
            );

        return await _signUpUserService.ExecuteAsync(
            request.Email,
            request.Password,
            request.FullName,
            request.CorrelationId,
            cancellationToken);
    }
}
