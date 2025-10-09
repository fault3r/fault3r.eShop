using System;
using CatalogManagementService.Domain.DTOs;
using CatalogManagementService.Domain.Entities;
using CatalogManagementService.Domain.Interfaces;
using CatalogManagementService.Infrastructure.Data.Contexts;
using CatalogManagementService.Infrastructure.Data.Documents;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CatalogManagementService.Infrastructure.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly MongoContext _context;

        private readonly FilterDefinitionBuilder<ItemDocument> filter;

        public ItemsRepository(MongoContext context)
        {
            _context = context;
            filter = Builders<ItemDocument>.Filter;
        }

        private static bool IdValidation(string id) =>
            ObjectId.TryParse(id, out var result);   

        public async Task<ItemsRepositoryResult> GetAllAsync()
        {
            try
            {
                var documents = await _context.Documents.Find(filter.Empty)
                    .ToListAsync();
                return new ItemsRepositoryResult
                {
                    Code = (int)ItemsRepositoryResultCode.Ok,
                    Items = documents.Select(i => i.ToDomain()),
                };
            }
            catch
            {
                return new ItemsRepositoryResult
                {
                    Code = (int)ItemsRepositoryResultCode.InternalServerError,
                };
            }
        }

        public async Task<ItemsRepositoryResult> GetByIdAsync(string id)
        {
            try
            {
                if(!IdValidation(id))
                    return new ItemsRepositoryResult
                    {
                        Code = (int)ItemsRepositoryResultCode.BadRequest,
                    };                
                var document = await _context.Documents
                    .Find(filter.Eq(p => p.Id, ObjectId.Parse(id)))
                    .FirstOrDefaultAsync();
                if (document is null)
                    return new ItemsRepositoryResult
                    {
                        Code = (int)ItemsRepositoryResultCode.NotFound
                    };
                return new ItemsRepositoryResult
                {
                    Code = (int)ItemsRepositoryResultCode.Ok,
                    Items = [document.ToDomain()],
                };
            }
            catch
            {
                return new ItemsRepositoryResult
                {
                    Code = (int)ItemsRepositoryResultCode.InternalServerError,
                };
            }
        }

        public async Task<ItemsRepositoryResult> CreateAsync(Item item)
        {
            try
            {
                var document = new ItemDocument
                {
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                };
                await _context.Documents.InsertOneAsync(document);
                return new ItemsRepositoryResult
                {
                    Code = (int)ItemsRepositoryResultCode.Created,
                    Items = [document.ToDomain()],
                };
            }
            catch
            {
                return new ItemsRepositoryResult
                {
                    Code = (int)ItemsRepositoryResultCode.InternalServerError,
                };
            }
        }

        public async Task<ItemsRepositoryResult> UpdateAsync(Item item)
        {
            try
            {
                if (!IdValidation(item.Id))
                    return new ItemsRepositoryResult
                    {
                        Code = (int)ItemsRepositoryResultCode.BadRequest,
                    };
                var document = new ItemDocument
                {
                    Id = ObjectId.Parse(item.Id),
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Updated = DateTime.UtcNow,
                };
                var updated = await _context.Documents.FindOneAndReplaceAsync(
                    filter.Eq(p => p.Id, document.Id), document);
                if (updated is null)
                    return new ItemsRepositoryResult
                    {
                        Code = (int)ItemsRepositoryResultCode.NotFound
                    };
                return new ItemsRepositoryResult
                {
                    Code = (int)ItemsRepositoryResultCode.Ok,
                    Items = [document.ToDomain()],
                };
            }
            catch
            {
                return new ItemsRepositoryResult
                {
                    Code = (int)ItemsRepositoryResultCode.InternalServerError,
                };
            }
        }

        public async Task<ItemsRepositoryResult> DeleteAsync(string id)
        {
            try
            {
                if (!IdValidation(id))
                    return new ItemsRepositoryResult
                    {
                        Code = (int)ItemsRepositoryResultCode.BadRequest,
                    };
                var result = await _context.Documents.DeleteOneAsync(
                    filter.Eq(p => p.Id, ObjectId.Parse(id)));
                if (result.DeletedCount == 0)
                    return new ItemsRepositoryResult
                    {
                        Code = (int)ItemsRepositoryResultCode.NotFound
                    };
                return new ItemsRepositoryResult
                {
                    Code = (int)ItemsRepositoryResultCode.NoContent,
                };
            }
            catch
            {
                return new ItemsRepositoryResult
                {
                    Code = (int)ItemsRepositoryResultCode.InternalServerError,
                };
            }
        }                 
        
    }
}