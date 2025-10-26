
using System;
using System.Linq.Expressions;
using AccountService.Domain.Entities.Base;

namespace AccountService.Application.Interfaces.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : BaseEntity
    {
        Task<(int Code, IEnumerable<TEntity>? Items)> GetAllAsync();

        Task<(int Code, TEntity? Item)> FindOneAsync(Expression<Func<TEntity, bool>> condition);
    }
}
