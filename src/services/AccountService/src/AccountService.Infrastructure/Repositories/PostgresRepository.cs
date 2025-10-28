
using System;
using System.Linq.Expressions;
using AccountService.Application.Interfaces.Services;
using AccountService.Domain.Entities.Base;
using AccountService.Domain.Enums;
using AccountService.Domain.Interfaces;
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

        public async Task<(ResultCode Code, IEnumerable<TEntity>? Entities)> GetAllAsync()
        {
            try
            {
                await _logger.LogInformation("fetching all items..");
                var entities = await context.ToListAsync();
                await _logger.LogInformation($"successfully retrieved {entities.Count} item(s).");
                return (ResultCode.Ok, entities);
            }
            catch
            {
                await _logger.LogError("failed to retrieve items!");
                return (ResultCode.InternalServerError, null);
            }
        }

        public async Task<(ResultCode Code, TEntity? Entity)> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                await _logger.LogInformation($"fetching item..");
                var entity = await context.FirstOrDefaultAsync(predicate);
                if (entity is null)
                {
                    await _logger.LogInformation($"no item found!");
                    return (ResultCode.NotFound, null);
                }
                await _logger.LogInformation($"successfully retrieved item.");
                return (ResultCode.Ok, entity);
            }
            catch
            {
                await _logger.LogError("failed to retrieve item!");
                return (ResultCode.InternalServerError, null);
            }
        }

        public async Task<(ResultCode Code, TEntity? Entity)> CreateAsync(TEntity entity)
        {
            try
            {
                await _logger.LogInformation($"creating item..");
                var created = context.Add(entity);
                await _context.SaveChangesAsync();
                await _logger.LogInformation($"item created.");
                return (ResultCode.Created, created.Entity);
            }
            catch
            {
                await _logger.LogError("failed to create item!");
                return (ResultCode.InternalServerError, null);
            }
        }

        public async Task<(ResultCode Code, TEntity? Entity)> UpdateAsync(TEntity entity)
        {
            try
            {
                await _logger.LogInformation($"updating item..");
                var updated = await context.FirstOrDefaultAsync(p => p.Id == entity.Id);
                if (updated is null)
                {
                    await _logger.LogInformation($"item not found!");
                    return (ResultCode.NotFound, null);
                }
                context.Entry(updated).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                await _logger.LogInformation($"item updated.");
                return (ResultCode.Ok, updated);
            }
            catch
            {
                await _logger.LogError("failed to update item!");
                return (ResultCode.InternalServerError, null);
            }
        }

        public async Task<ResultCode> DeleteAsync(int id)
        {
            try
            {
                await _logger.LogInformation($"deleting item..");
                var entity = await context.FirstOrDefaultAsync(p => p.Id == id);
                if (entity is null)
                {
                    await _logger.LogInformation($"item not found!");
                    return ResultCode.NotFound;
                }
                context.Remove(entity);
                await _context.SaveChangesAsync();
                await _logger.LogInformation($"item deleted.");
                return ResultCode.NoContent;
            }
            catch
            {
                await _logger.LogError("failed to delete item!");
                return ResultCode.InternalServerError;
            }
        }
    }
}