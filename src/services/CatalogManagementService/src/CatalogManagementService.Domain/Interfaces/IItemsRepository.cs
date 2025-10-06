
using System;
using CatalogManagementService.Domain.DTOs;
using CatalogManagementService.Domain.Entities;

namespace CatalogManagementService.Domain.Interfaces
{
    public interface IItemsRepository
    {
        Task<ItemsRepositoryResult> GetAllAsync();

        Task<ItemsRepositoryResult> GetByIdAsync(string id);

        Task<ItemsRepositoryResult> CreateAsync(Item item);

        Task<ItemsRepositoryResult> UpdateAsync(Item item);

        Task<ItemsRepositoryResult> DeleteAsync(string id);
    }
}