
using System;
using AccountService.Application.Interfaces.Repositories;
using AccountService.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/account")]
    public class AccountController : ControllerBase
    {
        private readonly IRepository _repo;

        private readonly ILoggerService<AccountController> _logger;


        public AccountController(IRepository repo,
            ILoggerService<AccountController> logger)
        {
            _repo = repo;
            _logger = logger;
            _logger.LogInformation("instance created.");
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _logger.LogInformation("forwarding req to repo.");
            var (code, accounts) = await _repo.GetAllAsync();
            await _logger.LogInformation("retrived res from repo.");
            return Ok($"Code: {code} - Accounts: {accounts}");
        }
    }
}
