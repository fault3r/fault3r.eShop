
using System;
using System.Linq.Expressions;
using AccountService.Domain.Entities;

namespace AccountService.Application.Interfaces.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : BaseEntity
    {
        Task<(int Code, IEnumerable<TEntity>?)> GetAllAsync();

        Task<(int Code, TEntity?)> FindOneAsync(Expression<Func<TEntity, bool>> condition);
    }
}
