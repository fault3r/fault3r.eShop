
using System;
using AccountService.Domain.Entities;

namespace AccountService.Application.Interfaces.Repositories
{
    public interface IRepository
    {

        Task<(RepositoryResult Code, IEnumerable<Account> Accounts)> GetAllAsync();

        public enum RepositoryResult
        {
            Ok = 200,
            Created = 201,
            NoContent = 204,
            BadRequest = 400,
            NotFound = 404,
            InternalServerError = 500,
        }
    }
}
