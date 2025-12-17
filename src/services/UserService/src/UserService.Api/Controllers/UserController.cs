
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
        _logger.LogInformation("SignUpUser request received for Email: {Email}.", request.Email);

        var command = new SignUpUserCommand(
            request.Email,
            request.Password,
            request.FullName,
            _correlation.CorrelationId
        );

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            _logger.LogWarning("SignUpUser request failed!");
            
            string error = result.Error ?? "unknown error";
            
            if (error.Contains("already exists"))
                return Conflict(new
                {
                    error,
                    correlationId = _correlation.CorrelationId
                });

            return BadRequest(new
            {
                error,
                correlationId = _correlation.CorrelationId
            });
        }

        _logger.LogInformation("SignUpUser request completed successfully.");

        return Ok(result.Value);
    }
}
