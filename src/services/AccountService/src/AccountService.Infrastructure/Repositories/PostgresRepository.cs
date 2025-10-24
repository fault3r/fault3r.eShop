
using System;
using AccountService.Application.Interfaces.Repositories;
using AccountService.Application.Interfaces.Services;
using AccountService.Domain.Entities;
using AccountService.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using static AccountService.Application.Interfaces.Repositories.IRepository;

namespace AccountService.Infrastructure.Repositories
{
    public class PostgresRepository : IRepository
    {
        private readonly PostgresDbContext _context;

        private readonly ILoggerService<PostgresRepository> _logger;

        public PostgresRepository(PostgresDbContext context,
            ILoggerService<PostgresRepository> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogInformation("instance created.");
        }

        public async Task<(RepositoryResult Code, IEnumerable<Account> Accounts)> GetAllAsync()
        {
            try
            {
                await _logger.LogInformation("fetching all accounts..");
                var accounts = (await _context.Accounts.ToListAsync())
                    .AsEnumerable();
                await _logger.LogInformation($"successfully retrieved {accounts.Count()} account(s).");
                return (RepositoryResult.Ok, accounts);
            }
            catch
            {
                await _logger.LogError("failed to retrieve accounts!");
                return (RepositoryResult.InternalServerError, null!);
            }
        }
    }
}
