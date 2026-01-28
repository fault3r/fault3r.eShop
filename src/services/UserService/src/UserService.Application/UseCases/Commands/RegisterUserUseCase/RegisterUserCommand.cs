
using System;
using MediatR;
using UserService.Domain.Contracts;

namespace UserService.Application.UseCases.Commands.RegisterUserUseCase;

public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string FullName
) : IRequest<Result<RegisterUserResult>>;