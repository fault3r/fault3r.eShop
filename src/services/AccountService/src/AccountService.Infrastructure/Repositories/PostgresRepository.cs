
using System;
using AccountService.Domain.Entities.Base;
using AccountService.Domain.Enums;
using AccountService.Domain.Interfaces;
using AccountService.Infrastructure.Data.Contexts;
using AccountService.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
            _logger.LogInformation($"initialized a repository for the {typeof(TEntity).Name} entity.");
        }

        public async Task<(ResultCode Code, IEnumerable<TEntity>? Entities)> GetAllAsync()
        {
            try
            {
                await _logger.LogInformation($"fetching all {typeof(TEntity).Name} entities..");
                var entities = await context.ToListAsync();
                await _logger.LogInformation($"successfully retrieved {entities.Count} {typeof(TEntity).Name}" +
                    $" entit{(entities.Count > 1 ? "ies" : "y")}.");                    
                return (ResultCode.Ok, entities);
            }
            catch
            {
                await _logger.LogError($"failed to retrieve {typeof(TEntity).Name} entites!");
                return (ResultCode.InternalServerError, null);
            }
        }

        public async Task<(ResultCode Code, TEntity? Entity)> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                await _logger.LogInformation($"fetching {typeof(TEntity).Name} entity for {predicate.Body}..");
                var entity = await context.FirstOrDefaultAsync(predicate);
                if (entity is null)
                {
                    await _logger.LogInformation($"no {typeof(TEntity).Name} entity found for {predicate.Body}!");
                    return (ResultCode.NotFound, null);
                }
                await _logger.LogInformation($"successfully retrieved {typeof(TEntity).Name} entity with id {entity.Id}.");
                return (ResultCode.Ok, entity);
            }
            catch
            {
                await _logger.LogError($"failed to retrieve {typeof(TEntity).Name} entity for {predicate.Body}..");
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