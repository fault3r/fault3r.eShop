
using System;
using AccountService.Application.Interfaces.Services;
using AccountService.Infrastructure.Data.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/account")]
    public class AccountController : ControllerBase
    {
        private readonly PostgresDbContext _context;

        private readonly ILoggerService<AccountController> _logger;

        public AccountController(PostgresDbContext context,
            ILoggerService<AccountController> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogInformation("instance created.");
        }
    }
}
