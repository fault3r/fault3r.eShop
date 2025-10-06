using System;
using CatalogManagementService.Application.DTOs;

namespace CatalogManagementService.Application.Interfaces
{
    public interface IItemsService
    {
        Task<(int Code, IEnumerable<ItemDto> Items)> GetAllAsync();

        Task<(int Code, ItemDto? Item)> GetByIdAsync(string id);

        Task<(int Code, ItemDto? Item)> CreateAsync(CreateItemDto item);

        Task<(int Code, ItemDto? Item)> UpdateAsync(string id, UpdateItemDto item);

        Task<int> DeleteAsync(string id);
    }
}