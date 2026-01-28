
using System;
using MediatR;
using UserService.Domain.Contracts;

namespace UserService.Application.UseCases.Commands.LogoutUserUseCase;

public sealed record LogoutUserCommand(
    string SessionId
) : IRequest<Result>;
