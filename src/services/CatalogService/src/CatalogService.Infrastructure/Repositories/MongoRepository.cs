
using System;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DTOs;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;
using CatalogService.Infrastructure.Data.Contexts;
using CatalogService.Infrastructure.Data.Documents;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CatalogService.Infrastructure.Repositories
{
    public class MongoRepository : IRepository
    {
        private readonly MongoContext _context;

        private readonly FilterDefinitionBuilder<ItemDocument> filter;

        private readonly ILoggerService<MongoRepository> _logger;

        public MongoRepository(MongoContext context,
            ILoggerService<MongoRepository> logger)
        {
            _context = context;
            filter = Builders<ItemDocument>.Filter;
            _logger = logger;
            _logger.LogInformation("instance created.");
        }

        public async Task<RepositoryResult> GetAllAsync()
        {
            try
            {
                await _logger.LogInformation("fetching all items..");
                var documents = await _context.Documents.Find(filter.Empty)
                    .ToListAsync();
                await _logger.LogInformation($"successfully retrieved {documents.Count} item(s).");
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.Ok,
                    Items = documents.Select(i => i.ToDomain()),
                };
            }
            catch
            {
                await _logger.LogError("failed to retrieve items!");
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.InternalServerError,
                };
            }
        }

        public async Task<RepositoryResult> GetByIdAsync(string id)
        {
            try
            {
                await _logger.LogInformation($"fetching item with id '{id}'..");
                var document = await _context.Documents
                    .Find(filter.Eq(p => p.Id, ObjectId.Parse(id)))
                    .FirstOrDefaultAsync();
                if (document is null)
                {
                    await _logger.LogInformation($"no item found with id '{id}'!");
                    return new RepositoryResult
                    {
                        Code = (int)RepositoryResultCode.NotFound
                    };
                }
                await _logger.LogInformation($"successfully retrieved item with id '{id}'.");
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.Ok,
                    Items = [document.ToDomain()],
                };
            }
            catch
            {
                await _logger.LogError("failed to retrieve item!");
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.InternalServerError,
                };
            }
        }

        public async Task<RepositoryResult> CreateAsync(Item item)
        {
            try
            {
                await _logger.LogInformation($"creating new item with name '{item.Name}'..");
                var document = new ItemDocument
                {
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                };
                await _context.Documents.InsertOneAsync(document);
                await _logger.LogInformation($"item created successfully with id '{document.ToDomain().Id}'.");
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.Created,
                    Items = [document.ToDomain()],
                };
            }
            catch
            {
                await _logger.LogError($"failed to create item!");
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.InternalServerError,
                };
            }
        }

        public async Task<RepositoryResult> UpdateAsync(Item item)
        {
            try
            {
                await _logger.LogInformation($"updating item with id '{item.Id}'..");
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
                {
                    await _logger.LogInformation($"no item found to update with id '{item.Id}'!");
                    return new RepositoryResult
                    {
                        Code = (int)RepositoryResultCode.NotFound
                    };
                }
                await _logger.LogInformation($"item with id '{item.Id}' updated successfully.");
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.Ok,
                    Items = [document.ToDomain()],
                };
            }
            catch
            {
                await _logger.LogError($"failed to update item!");
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.InternalServerError,
                };
            }
        }

        public async Task<RepositoryResult> DeleteAsync(string id)
        {
            try
            {
                await _logger.LogInformation($"deleting item with id '{id}'..");
                var result = await _context.Documents.DeleteOneAsync(
                    filter.Eq(p => p.Id, ObjectId.Parse(id)));
                if (result.DeletedCount == 0)
                {
                    await _logger.LogInformation($"no item found to delete with id '{id}'!");
                    return new RepositoryResult
                    {
                        Code = (int)RepositoryResultCode.NotFound
                    };
                }
                await _logger.LogInformation($"item with id '{id}' deleted successfully.");
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.NoContent,
                };
            }
            catch
            {
                await _logger.LogError($"failed to delete item!");
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.InternalServerError,
                };
            }
        }                 
    }
}