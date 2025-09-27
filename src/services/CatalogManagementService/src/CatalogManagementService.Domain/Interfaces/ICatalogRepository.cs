
using System;
using CatalogManagementService.Domain.DTOs;
using CatalogManagementService.Domain.Entities;

namespace CatalogManagementService.Domain.Interfaces
{
    public interface ICatalogRepository
    {
        Task<RepositoryResult> GetAllAsync();

        Task<RepositoryResult> GetByIdAsync(string id);

        Task<RepositoryResult> CreateAsync(Item item);

        Task<RepositoryResult> UpdateAsync(Item item);

        Task<RepositoryResult> DeleteAsync(string id);
    }
}