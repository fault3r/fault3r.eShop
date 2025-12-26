
using System;
using MediatR;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.SignUpUser;

public sealed record SignUpUserCommand(
    string Email,
    string Password,
    string FullName,
    string CorrelationId
) : IRequest<Result<User>>;