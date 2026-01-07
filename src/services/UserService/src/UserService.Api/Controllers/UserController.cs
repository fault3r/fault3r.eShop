
using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using UserService.Api.DTOs;
using UserService.Application.UseCases.Commands.LoginUserUseCase;
using UserService.Application.UseCases.Commands.RefreshAuthUseCase;
using UserService.Application.UseCases.Commands.RegisterUserUseCase;
using UserService.Application.UseCases.Queries.UserProfileUseCase;
using UserService.Infrastructure.DependencyInjection;

namespace UserService.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/user")]
public sealed class UserController(
    IMediator mediator
) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var command = new RegisterUserCommand(
            Email: request.Email,
            Password: request.Password,
            FullName: request.FullName
        );

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            string error = result.Error ?? "unknown error";

            if (error.Contains("already exists"))
                return Conflict(new { errorMessage = error });

            return BadRequest(new { errorMessage = error });
        }

        return Ok(result.Value);
    }

    [HttpPost]
    //[Route("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserDto request)
    {

    }

    [HttpPost]
    [Route("/auth/refresh")]
    [EnableRateLimiting("AuthRateLimit")]
    public async Task<IActionResult> RefreshAuth([FromBody] RefreshAuthDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var command = new RefreshAuthCommand(
            AccessToken: request.AccessToken,
            RefreshToken: request.RefreshToken
        );

        var result = await _mediator.Send(command);
     
        return result.IsFailure
            ? BadRequest(new { errorMessage = result.Error })
            : Ok(result.Value);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> ProfileAsync()
    {
        string? sessionId = HttpContext.SessionId();
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

        var query = new UserProfileQuery(
            SessionId: sessionId
        );

        var result = await _mediator.Send(query);

        return result.IsFailure
            ? Unauthorized(new { errorMessage = result.Error })
            : Ok(result.Value);
    }
}
