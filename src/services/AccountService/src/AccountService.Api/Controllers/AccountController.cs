
using System;
using AccountService.Application.Interfaces.Services;
using AccountService.Domain.Entities;
using AccountService.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/account")]
    public class AccountController : ControllerBase
    {
        private readonly IRepository<Account> _repo;

        private readonly ILoggerService<AccountController> _logger;

        public AccountController(IRepository<Account> repo,
            ILoggerService<AccountController> logger)
        {
            _repo = repo;
            _logger = logger;
            _logger.LogInformation("instance created.");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var (codee, items) = await _repo.GetAllAsync();
            var (code, item) = await _repo.GetAsync(p => p.Name == "Hamed");
            return Ok(item);
        }
    }
}
