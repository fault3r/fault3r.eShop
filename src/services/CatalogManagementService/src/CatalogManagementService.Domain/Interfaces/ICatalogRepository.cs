
using System;
using CatalogManagementService.Domain.Entities;

namespace CatalogManagementService.Domain.Interfaces
{
    public interface ICatalogManagementRepository
    {
        Task<IEnumerable<Item>> GetAllAsync();

        Task<(bool Success, Item? Item)> GetByIdAsync(string id);

        Task<(bool Success, Item? Item)> CreateAsync(Item item);

        Task<(bool Success, Item? Item)> UpdateAsync(Item item);

        Task<bool> DeleteAsync(string id);
    }
}