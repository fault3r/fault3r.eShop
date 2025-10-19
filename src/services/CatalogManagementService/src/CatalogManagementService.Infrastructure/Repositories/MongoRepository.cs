
using System;
using CatalogManagementService.Application.Interfaces;
using CatalogManagementService.Domain.DTOs;
using CatalogManagementService.Domain.Entities;
using CatalogManagementService.Domain.Interfaces;
using CatalogManagementService.Infrastructure.Data.Contexts;
using CatalogManagementService.Infrastructure.Data.Documents;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CatalogManagementService.Infrastructure.Repositories
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
            _logger.LogInformation("Repository initialized successfully.");
        }

        private static bool IdValidate(string id) =>
            ObjectId.TryParse(id, out var result);   

        public async Task<RepositoryResult> GetAllAsync()
        {
            await _logger.LogInformation("Fetching all items..");
            try
            {
                var documents = await _context.Documents.Find(filter.Empty)
                    .ToListAsync();
                await _logger.LogInformation($"Successfully retrieved {documents.Count} items.");
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.Ok,
                    Items = documents.Select(i => i.ToDomain()),
                };
            }
            catch
            {
                await _logger.LogInformation("Failed to retrieve items!");
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.InternalServerError,
                };
            }
        }

        public async Task<RepositoryResult> GetByIdAsync(string id)
        {
            await _logger.LogInformation($"Fetching item with id: {id}");
            try
            {
                if (!IdValidate(id))
                {
                    await _logger.LogInformation($"Invalid: {id}");
                    return new RepositoryResult
                    {
                        Code = (int)RepositoryResultCode.BadRequest,
                    };
                }
                var document = await _context.Documents
                    .Find(filter.Eq(p => p.Id, ObjectId.Parse(id)))
                    .FirstOrDefaultAsync();
                if (document is null)
                    return new RepositoryResult
                    {
                        Code = (int)RepositoryResultCode.NotFound
                    };
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.Ok,
                    Items = [document.ToDomain()],
                };
            }
            catch
            {
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.InternalServerError,
                };
            }
        }

        public async Task<RepositoryResult> CreateAsync(Item item)
        {
            await _logger.LogInformation($"Creating new item with name: {item.Name}");
            try
            {
                var document = new ItemDocument
                {
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                };
                await _context.Documents.InsertOneAsync(document);
                await _logger.LogInformation($"Created successfully with id: {document.ToDomain().Id}");
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.Created,
                    Items = [document.ToDomain()],
                };
            }
            catch
            {
                await _logger.LogInformation($"Failed to create item with name: {item.Name}");
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.InternalServerError,
                };
            }
        }

        public async Task<RepositoryResult> UpdateAsync(Item item)
        {
            //log           
            Console.WriteLine($"***{nameof(MongoRepository)} is running an update operation.");
            try
            {
                if (!IdValidate(item.Id))
                    return new RepositoryResult
                    {
                        Code = (int)RepositoryResultCode.BadRequest,
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
                    return new RepositoryResult
                    {
                        Code = (int)RepositoryResultCode.NotFound
                    };
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.Ok,
                    Items = [document.ToDomain()],
                };
            }
            catch
            {
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.InternalServerError,
                };
            }
        }

        public async Task<RepositoryResult> DeleteAsync(string id)
        {
            //log           
            Console.WriteLine($"***{nameof(MongoRepository)} is running a delete operation.");
            try
            {
                if (!IdValidate(id))
                    return new RepositoryResult
                    {
                        Code = (int)RepositoryResultCode.BadRequest,
                    };
                var result = await _context.Documents.DeleteOneAsync(
                    filter.Eq(p => p.Id, ObjectId.Parse(id)));
                if (result.DeletedCount == 0)
                    return new RepositoryResult
                    {
                        Code = (int)RepositoryResultCode.NotFound
                    };
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.NoContent,
                };
            }
            catch
            {
                return new RepositoryResult
                {
                    Code = (int)RepositoryResultCode.InternalServerError,
                };
            }
        }                 
        
    }
}