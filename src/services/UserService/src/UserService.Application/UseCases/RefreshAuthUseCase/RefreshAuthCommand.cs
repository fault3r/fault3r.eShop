
using System;
using MediatR;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.RefreshAuthUseCase;

public sealed record RefreshAuthCommand(
    string AccessToken,
    string RefreshToken
) : IRequest<Result<RefreshAuthResult>>;
