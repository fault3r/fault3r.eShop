
using System;
using AccountService.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/account")]
    public class AccountController : ControllerBase
    {
        private readonly ILoggerService<AccountController> _logger;

        public AccountController(
            ILoggerService<AccountController> logger)
        {
            _logger = logger;
            _logger.LogInformation("instance created.");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }
    }
}
