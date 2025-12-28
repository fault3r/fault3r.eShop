
using System;
using MediatR;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.UserAggregate.SignUpUser;

public sealed record SignUpUserCommand(
    string Email,
    string Password,
    string FullName
) : IRequest<Result<User>>;