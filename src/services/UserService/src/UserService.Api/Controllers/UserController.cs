

using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Api.DTOs.User;
using UserService.Application.UseCases.SignUpUser;
using UserService.Infrastructure.CrossCutting;

namespace UserService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICorrelationContext _correlation;

    public UserController(IMediator mediator, ICorrelationContext correlationContext)
    {
        _mediator = mediator;
        _correlation = correlationContext;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(SignUpUserDto request)
    {
        var correlationId = _correlation.CorrelationId;

        var command = new SignUpUserCommand(
            request.Email,
            request.Password,
            request.FullName,
            correlationId
        );

        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }
}
