
using System;
using CatalogManagementService.Application.DTOs;

namespace CatalogManagementService.Application.Interfaces
{
    public interface IGetItemService
    {
        Task<(int Code, ItemDto? Item)> ExecuteAsync(string id);
    }
}
