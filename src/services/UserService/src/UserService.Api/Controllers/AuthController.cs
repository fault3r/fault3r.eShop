
using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using UserService.Api.DTOs;
using UserService.Application.UseCases.Commands.LoginUserUseCase;
using UserService.Application.UseCases.Commands.RefreshAuthUseCase;
using UserService.Application.UseCases.Commands.RegisterUserUseCase;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/auth")]
[ApiVersion("1.0")]
public sealed class AuthController(
    IMediator mediator
) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [Route("register")]
    [MapToApiVersion("1.0")]
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

        return Created(string.Empty, result.Value);
    }

    [HttpPost]
    [Route("login")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var command = new LoginUserCommand(
            Identity: request.Identity,
            Password: request.Password
        );

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            string error = result.Error ?? "unknown error";

            if (error.Contains("credentials"))
                return Unauthorized(new { errorMessage = error });

            if (error.Contains("locked"))
                return StatusCode(StatusCodes.Status403Forbidden, new { errorMessage = error });
            
            return BadRequest(new { errorMessage = error });
        }

        return Ok(result.Value);
    }

    [HttpPost]
    [Route("refresh")]
    [MapToApiVersion("1.0")]
    [EnableRateLimiting("RefAuthRateLimit")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshAuthDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var command = new RefreshAuthCommand(
            AccessToken: request.AccessToken,
            RefreshToken: request.RefreshToken
        );

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            string error = result.Error ?? "unknown error";

            if (error.Contains("Invalid") || error.Contains("expired"))
                return Unauthorized(new { errorMessage = error });

            return BadRequest(new { errorMessage = error });
        }
        
        return Ok(result.Value);
    }

    [Authorize]
    [HttpPost]
    [Route("logout")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> LogoutAsync()
    {

        return Ok();
    }
}



