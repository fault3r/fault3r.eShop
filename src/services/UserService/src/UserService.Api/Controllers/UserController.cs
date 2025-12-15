
using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Api.DTOs.User;
using UserService.Application.UseCases.SignUpUser;
using UserService.Infrastructure.CrossCutting;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICorrelationContext _correlation;
    private readonly ILogger<UserController> _logger;

    public UserController(
        IMediator mediator,
        ICorrelationContext correlationContext,
        ILogger<UserController> logger)
    {
        _mediator = mediator;
        _correlation = correlationContext;
        _logger = logger;
    }

    [HttpGet]
    [Route("test")]
    public async Task<IActionResult> TestMethod()
        => Ok("access granted");

    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpUserDto request)
    {
        _logger.LogInformation(
            "SignUpUser request received for email:{Email}", request.Email);

        var command = new SignUpUserCommand(
            request.Email,
            request.Password,
            request.FullName,
            _correlation.CorrelationId
        );

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            _logger.LogWarning(
                "SignUpUser request failed for email:{Email}, Error(s):{Error}", request.Email, result.Error);

            return BadRequest(result.Error);
        }

        _logger.LogInformation(
            "SignUpUser request complete successfully with email:{Email}", request.Email);

        return Ok(result.Value);
    }
}
