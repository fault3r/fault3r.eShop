
using System;

namespace CatalogManagementService.Application.Interfaces
{
    public interface IDeleteItemService
    {
        Task<int> ExecuteAsync(string id);
    }
}
