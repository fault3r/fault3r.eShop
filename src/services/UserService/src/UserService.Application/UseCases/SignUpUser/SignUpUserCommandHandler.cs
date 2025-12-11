
using System;
using MediatR;
using UserService.Domain.Aggregates;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.SignUpUser;

public class SignUpUserCommandHandler
    : IRequestHandler<SignUpUserCommand, Result<User>>
{
    public async Task<Result<User>> Handle(SignUpUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
