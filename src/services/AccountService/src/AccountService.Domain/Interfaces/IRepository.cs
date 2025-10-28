
using System;
using System.Linq.Expressions;
using AccountService.Domain.Entities.Base;
using AccountService.Domain.Enums;

namespace AccountService.Domain.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : BaseEntity
    {
        Task<(ResultCode Code, IEnumerable<TEntity>? Entities)> GetAllAsync();

        Task<(ResultCode Code, TEntity? Entity)> GetAsync(Expression<Func<TEntity, bool>> predicate);

        Task<(ResultCode Code, TEntity? Entity)> CreateAsync(TEntity entity);

        Task<(ResultCode Code, TEntity? Entity)> UpdateAsync(TEntity entity);

        Task<ResultCode> DeleteAsync(int id);
    }
}
