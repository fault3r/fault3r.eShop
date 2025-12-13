
using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Api.DTOs.User;
using UserService.Application.UseCases.SignUpUser;
using UserService.Infrastructure.CrossCutting;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICorrelationContext _correlation;

    public UserController(IMediator mediator, ICorrelationContext correlationContext)
    {
        _mediator = mediator;
        _correlation = correlationContext;
    }

    [HttpPost]
    [Route("SignUp")]
    public async Task<IActionResult> SignUp([FromBody] SignUpUserDto request)
    {
        var command = new SignUpUserCommand(
            request.Email,
            request.Password,
            request.FullName,
            _correlation.CorrelationId
        );

        var result = await _mediator.Send(command);

        return result.IsFailure
            ? BadRequest(result.Error)
            : Ok(result.Value);
    }
}
