
using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Api.DTOs;
using UserService.Application.UseCases.LoginUserUseCase;
using UserService.Application.UseCases.RefreshAuthUseCase;
using UserService.Application.UseCases.RegisterUserUseCase;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/user")]
public sealed class UserController(IMediator mediator) : ControllerBase
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
    [Route("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var command = new LoginUserCommand(
            Identity: request.Identity,
            Password: request.Password
        );

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(new { errorMessage = result.Error });

        return Ok(result.Value);
    }

    [HttpPost]
    [Route("/auth/refresh")]
    public async Task<IActionResult> RefreshAuth([FromBody] RefreshAuthDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var command = new RefreshAuthCommand(
            AccessToken: request.ExpiredAccessToken,
            RefreshToken: request.ProvidedRefreshToken
        );

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(new { errorMessage = result.Error });

        return Ok(result.Value);
    }

    [Authorize]
    [HttpGet]
    [Route("profile")]
    public async Task<IActionResult> ProfileAsync()
    {
        return Ok("Welcome, User!");
    }
}
