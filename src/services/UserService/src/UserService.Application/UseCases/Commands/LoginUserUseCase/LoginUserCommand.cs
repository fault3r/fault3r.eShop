
using System;
using MediatR;
using UserService.Domain.Contracts;

namespace UserService.Application.UseCases.Commands.LoginUserUseCase;

public sealed record LoginUserCommand(
    string Identity,
    string Password
) : IRequest<Result<LoginUserResult>>;