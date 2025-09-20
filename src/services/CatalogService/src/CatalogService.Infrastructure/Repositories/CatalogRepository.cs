using System;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;
using CatalogService.Infrastructure.Data.Contexts;
using CatalogService.Infrastructure.Data.Documents;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CatalogService.Infrastructure.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly CatalogContext _context;

        private readonly FilterDefinitionBuilder<ItemDocument> filter;

        public CatalogRepository(CatalogContext context)
        {
            _context = context;
            filter = Builders<ItemDocument>.Filter;
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            var documents = await _context.Documents.Find(filter.Empty)
                .ToListAsync();
            return documents.Select(x => x.ToDomain());
        }

        public async Task<(bool Success, Item? Item)> GetByIdAsync(string id)
        {
            var document = await _context.Documents.Find(filter.Eq(p => p.Id, ObjectId.Parse(id)))
                .FirstOrDefaultAsync();
            if (document is null)
                return (Success: false, Item: null);
            return (Success: true, Item: document.ToDomain());
        }
    }
}