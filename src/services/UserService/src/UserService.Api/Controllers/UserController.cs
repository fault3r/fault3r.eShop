
using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Api.DTOs.User;
using UserService.Application.UseCases.UserAggregate.SignUpUser;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/user")]
public sealed class UserController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpUserDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var command = new SignUpUserCommand(
            request.Email,
            request.Password,
            request.FullName
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
}
