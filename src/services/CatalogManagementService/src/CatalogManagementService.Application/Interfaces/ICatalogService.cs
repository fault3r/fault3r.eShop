using System;
using CatalogManagementService.Application.DTOs;

namespace CatalogManagementService.Application.Interfaces
{
    public interface ICatalogService
    {
        Task<IEnumerable<ItemDto>> GetAllAsync();

        Task<(bool Success, ItemDto? Item)> GetByIdAsync(string id);

        Task<(bool Success, ItemDto? Item)> CreateAsync(CreateItemDto item);

        Task<(bool Success, ItemDto? Item)> UpdateAsync(UpdateItemDto item);

        Task<bool> DeleteAsync(string id);
    }
}