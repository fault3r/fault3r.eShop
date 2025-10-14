
using System;
using CatalogManagementService.Application.DTOs;

namespace CatalogManagementService.Application.Interfaces
{
    public interface ICreateItemService
    {
        Task<(int Code, ItemDto? Item)> ExecuteAsync(CreateItemDto item);
    }
}
