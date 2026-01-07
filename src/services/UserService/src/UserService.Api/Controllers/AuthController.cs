
using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Api.DTOs;
using UserService.Application.UseCases.Commands.LoginUserUseCase;

namespace UserService.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public sealed class AuthController(
    IMediator mediator
) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("login")]
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

            if (error.Contains("locked"))
                return Forbid();

            return BadRequest(new { errorMessage = error });
        }

        return Ok(result.Value);
    }


    [HttpPost("register")] public async Task<IActionResult> RegisterAsync(RegisterUserDto dto) { ... }
    [HttpPost("refresh")] public async Task<IActionResult> RefreshAsync(RefreshAuthDto dto) { ... }
    [Authorize][HttpPost("logout")] public async Task<IActionResult> LogoutAsync() { ... }


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