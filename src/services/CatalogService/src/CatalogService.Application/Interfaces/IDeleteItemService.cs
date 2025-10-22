
using System;

namespace CatalogService.Application.Interfaces
{
    public interface IDeleteItemService
    {
        Task<int> ExecuteAsync(string id);
    }
}
