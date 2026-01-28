
using System;
using MediatR;
using UserService.Domain.Contracts;

namespace UserService.Application.UseCases.Queries.UserProfileUseCase;

public sealed record UserProfileQuery(
    string SessionId
) : IRequest<Result<UserProfileResult>>;
