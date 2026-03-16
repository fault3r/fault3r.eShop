
using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.UseCases.Queries.UserProfileUseCase;
using UserService.Infrastructure.DependencyInjection;

namespace UserService.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/users")]
public sealed class UserController(
    IMediator mediator
) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [Authorize]
    [HttpGet]
    [Route("me")]
    public async Task<IActionResult> ProfileAsync()
    {
        string sessionId = HttpContext.SessionId() ?? string.Empty;

        var query = new UserProfileQuery(
            SessionId: sessionId
        );

        var result = await _mediator.Send(query, HttpContext.RequestAborted);

        return result.IsFailure
            ? Unauthorized(new { errorMessage = result.Error })
            : Ok(result.Value);
    }
}



//         [Authorize][HttpPut("me/email")] public async Task<IActionResult> ChangeEmailAsync(ChangeEmailDto dto) { ... }
//         [Authorize][HttpPut("me/fullname")] public async Task<IActionResult> ChangeFullNameAsync(ChangeFullNameDto dto) { ... }
//         [Authorize][HttpPut("me/password")] public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto dto) { ... }
//         [HttpPost("reset-password")] public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDto dto) { ... }
