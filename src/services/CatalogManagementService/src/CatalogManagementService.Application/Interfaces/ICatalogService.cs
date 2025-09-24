using System;
using CatalogManagementService.Application.DTOs;

namespace CatalogManagementService.Application.Interfaces
{
    public interface ICatalogService
    {
        Task<IEnumerable<ItemDto>> GetAllAsync();

        Task<ItemDto?> GetByIdAsync(string id);

        Task<ItemDto?> CreateAsync(CreateItemDto item);

        Task<ItemDto?> UpdateAsync(string id, UpdateItemDto item);

        Task<bool> DeleteAsync(string id);
    }
}