
using System;
using MediatR;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.Queries.UserProfileUseCase;

public sealed record UserProfileQuery(
    string SessionId
) : IRequest<Result<UserProfileResult>>;
