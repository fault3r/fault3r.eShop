
using System;
using CatalogService.Domain.DTOs;
using CatalogService.Domain.Entities;

namespace CatalogService.Domain.Interfaces
{
    public interface IRepository
    {
        Task<RepositoryResult> GetAllAsync();

        Task<RepositoryResult> GetByIdAsync(string id);

        Task<RepositoryResult> CreateAsync(Item item);

        Task<RepositoryResult> UpdateAsync(Item item);

        Task<RepositoryResult> DeleteAsync(string id);
    }
}