
using System;
using System.Linq.Expressions;
using AccountService.Application.Interfaces.Repositories;
using AccountService.Application.Interfaces.Services;
using AccountService.Domain.Entities;
using AccountService.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Repositories
{
    public class PostgresRepository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly PostgresDbContext _context;

        private readonly ILoggerService<PostgresRepository<TEntity>> _logger;

        public PostgresRepository(PostgresDbContext context,
            ILoggerService<PostgresRepository<TEntity>> logger)
        {
            _context = context;
            _logger = logger;            
            _logger.LogInformation("instance created.");
        }

        public async Task<(int Code, IEnumerable<TEntity>?)> GetAllAsync()
        {
            try
            {
                await _logger.LogInformation("fetching all items..");
                var items = await _context.Set<TEntity>().ToListAsync();
                await _logger.LogInformation($"successfully retrieved {items.Count} item(s).");
                return (200, items);
            }
            catch
            {
                await _logger.LogError("failed to retrieve items!");
                return (500, null);
            }
        }

        public async Task<(int Code, TEntity?)> FindOneAsync(Expression<Func<TEntity, bool>> condition)
        {
            try
            {
                await _logger.LogInformation($"fetching item..");
                var item = await _context.Set<TEntity>().FirstOrDefaultAsync(condition);
                if (item is null)
                {
                    await _logger.LogInformation($"no item found!");
                    return (404, null);
                }
                await _logger.LogInformation($"successfully retrieved item.");
                return (200, item);
            }
            catch
            {
                await _logger.LogError("failed to retrieve item!");
                return (500, null);
            }
        }
    }
}