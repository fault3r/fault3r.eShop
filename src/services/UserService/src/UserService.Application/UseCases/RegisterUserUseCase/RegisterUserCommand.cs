
using System;
using MediatR;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.RegisterUserUseCase;

public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string FullName
) : IRequest<Result<User>>;