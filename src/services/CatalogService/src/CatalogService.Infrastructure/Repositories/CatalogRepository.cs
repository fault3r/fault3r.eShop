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

        public async Task<(bool Success, Item? Item)> CreateAsync(Item item)
        {
            var document = new ItemDocument
            {
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Pictures = item.Pictures,
            };
            await _context.Documents.InsertOneAsync(document);
            if (document.Id == ObjectId.Empty)
                return (Success: false, Item: null);
            return (Success: true, Item: document.ToDomain());
        }

        public async Task<(bool Success, Item? Item)> UpdateAsync(Item item)
        {
            var document = new ItemDocument
            {
                Id = ObjectId.Parse(item.Id),
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Pictures = item.Pictures,
                Updated = DateTime.UtcNow,
            };
            var updated = await _context.Documents.FindOneAndReplaceAsync(
                filter.Eq(p => p.Id, document.Id), document);
            if (updated is null)
                return (Success: false, Item: null);
            return (Success: true, Item: document.ToDomain());
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _context.Documents.DeleteOneAsync(
                filter.Eq(p => p.Id, ObjectId.Parse(id)));
            return result.DeletedCount == 0;
        }
    }
}