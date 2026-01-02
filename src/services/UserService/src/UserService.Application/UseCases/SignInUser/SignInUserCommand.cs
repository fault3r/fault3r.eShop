
using System;
using MediatR;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.UserAggregate.SignInUser;

public sealed record SignInUserCommand(
    string Identity,
    string Password
) : IRequest<Result<SignInUserResult>>;