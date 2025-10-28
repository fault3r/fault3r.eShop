
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

        private readonly string entity;

        private readonly ILoggerService<PostgresRepository<TEntity>> _logger;

        public PostgresRepository(PostgresDbContext context,
            ILoggerService<PostgresRepository<TEntity>> logger)
        {
            _context = context;
            this.context = _context.Set<TEntity>();
            entity = typeof(TEntity).Name;
            _logger = logger;
            _logger.LogInformation($"initialized a repository for the {this.entity} entity.");
        }

        public async Task<(ResultCode Code, IEnumerable<TEntity>? Entities)> GetAllAsync()
        {
            try
            {
                await _logger.LogInformation($"fetching all {this.entity} entities..");
                var entities = await context.ToListAsync();
                await _logger.LogInformation($"successfully retrieved {entities.Count} {this.entity} entit{(entities.Count > 1 ? "ies" : "y")}.");                    
                return (ResultCode.Ok, entities);
            }
            catch
            {
                await _logger.LogError($"failed to retrieve {this.entity} entites!");
                return (ResultCode.InternalServerError, null);
            }
        }

        public async Task<(ResultCode Code, TEntity? Entity)> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                await _logger.LogInformation($"fetching {this.entity} entity for {predicate.Body}..");
                var entity = await context.FirstOrDefaultAsync(predicate);
                if (entity is null)
                {
                    await _logger.LogInformation($"no {this.entity} entity found for {predicate.Body}!");
                    return (ResultCode.NotFound, null);
                }
                await _logger.LogInformation($"successfully retrieved {this.entity} entity with id {entity.Id}.");
                return (ResultCode.Ok, entity);
            }
            catch
            {
                await _logger.LogError($"failed to retrieve {this.entity} entity for {predicate.Body}!");
                return (ResultCode.InternalServerError, null);
            }
        }

        public async Task<(ResultCode Code, TEntity? Entity)> CreateAsync(TEntity entity)
        {
            try
            {
                await _logger.LogInformation($"creating a new {this.entity} entity..");
                var created = context.Add(entity);
                await _context.SaveChangesAsync();
                await _logger.LogInformation($"successfully created {this.entity} entity with id {created.Entity.Id}.");
                return (ResultCode.Created, created.Entity);
            }
            catch
            {
                await _logger.LogError($"failed to create {this.entity} entity!");
                return (ResultCode.InternalServerError, null);
            }
        }

        public async Task<(ResultCode Code, TEntity? Entity)> UpdateAsync(TEntity entity)
        {
            try
            {
                await _logger.LogInformation($"updating {this.entity} entity with id {entity.Id}..");
                var existing = await context.FirstOrDefaultAsync(p => p.Id == entity.Id);
                if (existing is null)
                {
                    await _logger.LogInformation($"no {this.entity} entity found for id {entity.Id}!");
                    return (ResultCode.NotFound, null);
                }
                context.Entry(existing).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                await _logger.LogInformation($"successfully updated {this.entity} entity with id {entity.Id}.");
                return (ResultCode.Ok, existing);
            }
            catch
            {
                await _logger.LogError($"failed to update {this.entity} entity with id {entity.Id}!");
                return (ResultCode.InternalServerError, null);
            }
        }

        public async Task<ResultCode> DeleteAsync(int id)
        {
            try
            {
                await _logger.LogInformation($"deleting {this.entity} entity with id {id}..");
                var entity = await context.FirstOrDefaultAsync(p => p.Id == id);
                if (entity is null)
                {
                    await _logger.LogInformation($"no {this.entity} entity found for id {id}!");
                    return ResultCode.NotFound;
                }
                context.Remove(entity);
                await _context.SaveChangesAsync();
                await _logger.LogInformation($"successfully deleted {this.entity} entity with id {id}.");
                return ResultCode.NoContent;
            }
            catch
            {
                await _logger.LogError($"failed to delete {this.entity} entity with id {id}!");
                return ResultCode.InternalServerError;
            }
        }
    }
}