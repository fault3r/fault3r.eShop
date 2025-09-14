using System;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;
using CatalogService.Infrastructure.Data.Contexts;
using CatalogService.Infrastructure.Data.Documents;
using MongoDB.Driver;

namespace CatalogService.Infrastructure.Repositories
{
    public class CatalogRepository: ICatalogRepository
    {
        private readonly CatalogContext _context;

        private FilterDefinitionBuilder<ItemDocument> filter;

        public CatalogRepository(CatalogContext context)
        {
            _context = context;
            filter = Builders<ItemDocument>.Filter;
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            var items = await _context.Items.Find(filter.Empty).ToListAsync();
            return items.Select(x => x.ToDomain());
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