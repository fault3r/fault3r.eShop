using System;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;
using CatalogService.Infrastructure.Data.Contexts;

namespace CatalogService.Infrastructure.Repositories
{
    public class CatalogRepository(CatalogContext context) : ICatalogRepository
    {
        private readonly CatalogContext context = context;

        public Task<IEnumerable<Item>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<(bool Success, Item? Item)> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<(bool Success, Item? Item)> CreateAsync(Item item)
        {
            throw new NotImplementedException();
        }

        public Task<(bool Success, Item? Item)> UpdateAsync(Item item)
        {
            throw new NotImplementedException();
        }
        
        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}