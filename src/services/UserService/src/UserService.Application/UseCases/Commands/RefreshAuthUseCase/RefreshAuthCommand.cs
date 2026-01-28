
using System;
using MediatR;
using UserService.Domain.Contracts;

namespace UserService.Application.UseCases.Commands.RefreshAuthUseCase;

public sealed record RefreshAuthCommand(
    string AccessToken,
    string RefreshToken
) : IRequest<Result<RefreshAuthResult>>;
