
using System;
using CatalogService.Domain.Entities;

namespace CatalogService.Domain.Interfaces
{
    public interface ICatalogRepository
    {
        Task<IEnumerable<Item>> GetAllAsync();

        Task<(bool Success, Item? Item)> GetByIdAsync(string id);
    }
}