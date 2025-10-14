
using System;
using CatalogManagementService.Application.DTOs;

namespace CatalogManagementService.Application.Interfaces
{
    public interface IUpdateItemService
    {
        Task<(int Code, ItemDto? Item)> ExecuteAsync(string id, UpdateItemDto item);
    }
}
