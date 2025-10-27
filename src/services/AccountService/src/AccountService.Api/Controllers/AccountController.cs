
using System;
using AccountService.Application.Interfaces.Repositories;
using AccountService.Application.Interfaces.Services;
using AccountService.Domain.Entities;
using AccountService.Infrastructure.Data.Contexts;
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
            // var (code, item) = await _repo.CreateAsync(new Account
            // {
            //     Name = "hamedt",
            //     Email = "hamed@ex.com",
            //     Password = "pswd",
            //     RoleId = 1,
            // });
            var (code, item) = await _repo.UpdateAsync(new Account
            {
                Id = 3,
                Name = "hamed-updated",
                Email = "email-updated",
                Password = "password-updated",
                RoleId = 2,
            });
            return Ok(item);
        }
    }
}
