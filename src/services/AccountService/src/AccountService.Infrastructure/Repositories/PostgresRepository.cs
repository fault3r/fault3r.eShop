
using System;
using System.Linq.Expressions;
using AccountService.Application.Interfaces.Repositories;
using AccountService.Application.Interfaces.Services;
using AccountService.Domain.Entities.Base;
using AccountService.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Repositories
{
    public class PostgresRepository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly PostgresDbContext _context;

        private readonly DbSet<TEntity> context;

        private readonly ILoggerService<PostgresRepository<TEntity>> _logger;

        public PostgresRepository(PostgresDbContext context,
            ILoggerService<PostgresRepository<TEntity>> logger)
        {
            _context = context;
            this.context = _context.Set<TEntity>();
            _logger = logger;
            _logger.LogInformation("instance created.");
        }

        public async Task<(int Code, IEnumerable<TEntity>? Items)> GetAllAsync()
        {
            try
            {
                await _logger.LogInformation("fetching all items..");
                var items = await context.ToListAsync();
                await _logger.LogInformation($"successfully retrieved {items.Count} item(s).");
                return (200, items);
            }
            catch
            {
                await _logger.LogError("failed to retrieve items!");
                return (500, null);
            }
        }

        public async Task<(int Code, TEntity? Item)> FindOneAsync(Expression<Func<TEntity, bool>> condition)
        {
            try
            {
                await _logger.LogInformation($"fetching item..");
                var item = await context.FirstOrDefaultAsync(condition);
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

        public async Task<(int Code, TEntity? Item)> CreateAsync(TEntity entity)
        {
            try
            {
                await _logger.LogInformation($"creating item..");
                var created = context.Add(entity);
                await _context.SaveChangesAsync();
                await _logger.LogInformation($"item created.");
                return (200, created.Entity);
            }
            catch
            {
                await _logger.LogError("failed to create item!");
                return (500, null);
            }
        }

        public async Task<(int Code, TEntity? Item)> UpdateAsync(TEntity entity)
        {
            try
            {
                await _logger.LogInformation($"updating item..");
                var item = await context.FirstOrDefaultAsync(x => x.Id == entity.Id);
                if (item is null)
                {
                    await _logger.LogInformation($"item not found!");
                    return (404, null);
                }
                context.Entry(item).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                await _logger.LogInformation($"item updated.");
                return (200, item);
            }
            catch
            {
                await _logger.LogError("failed to update item!");
                return (500, null);
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                await _logger.LogInformation($"deleting item..");
                var item = await context.FirstOrDefaultAsync(x => x.Id == id);
                if (item is null)
                {
                    await _logger.LogInformation($"item not found!");
                    return (404);
                }
                context.Remove(item);
                await _context.SaveChangesAsync();
                await _logger.LogInformation($"item deleted.");
                return (204);
            }
            catch
            {
                await _logger.LogError("failed to delete item!");
                return (500);
            }            
        }
    }
}