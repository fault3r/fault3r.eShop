
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


//   [ApiController]
//     [ApiVersion("1.0")]
//     [Route("api/v{version:apiVersion}/users")]
//     public sealed class UsersController(IMediator mediator) : ControllerBase
//     {
//         [Authorize][HttpGet("me")] public async Task<IActionResult> GetProfileAsync() { ... }
//         [Authorize][HttpPut("me/email")] public async Task<IActionResult> ChangeEmailAsync(ChangeEmailDto dto) { ... }
//         [Authorize][HttpPut("me/fullname")] public async Task<IActionResult> ChangeFullNameAsync(ChangeFullNameDto dto) { ... }
//         [Authorize][HttpPut("me/password")] public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto dto) { ... }
//         [HttpPost("reset-password")] public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDto dto) { ... }
//     }