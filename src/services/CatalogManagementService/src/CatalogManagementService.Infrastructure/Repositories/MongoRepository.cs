
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
    public class MongoRepository : IRepository
    {
        private readonly MongoContext _context;

        private readonly FilterDefinitionBuilder<ItemDocument> filter;

        public MongoRepository(MongoContext context)
        {
            //log           
            Console.WriteLine($"***{nameof(MongoRepository)} is initializing.");
            _context = context;
            filter = Builders<ItemDocument>.Filter;
        }

        private static bool IdValidation(string id) =>
            ObjectId.TryParse(id, out var result);   

        public async Task<MongoRepositoryResult> GetAllAsync()
        {
            //log           
            Console.WriteLine($"***{nameof(MongoRepository)} is running a getall operation.");
            try
            {
                var documents = await _context.Documents.Find(filter.Empty)
                    .ToListAsync();
                return new MongoRepositoryResult
                {
                    Code = (int)MongoRepositoryResultCode.Ok,
                    Items = documents.Select(i => i.ToDomain()),
                };
            }
            catch
            {
                return new MongoRepositoryResult
                {
                    Code = (int)MongoRepositoryResultCode.InternalServerError,
                };
            }
        }

        public async Task<MongoRepositoryResult> GetByIdAsync(string id)
        {
            //log           
            Console.WriteLine($"***{nameof(MongoRepository)} is running a get operation.");
            try
            {
                if(!IdValidation(id))
                    return new MongoRepositoryResult
                    {
                        Code = (int)MongoRepositoryResultCode.BadRequest,
                    };                
                var document = await _context.Documents
                    .Find(filter.Eq(p => p.Id, ObjectId.Parse(id)))
                    .FirstOrDefaultAsync();
                if (document is null)
                    return new MongoRepositoryResult
                    {
                        Code = (int)MongoRepositoryResultCode.NotFound
                    };
                return new MongoRepositoryResult
                {
                    Code = (int)MongoRepositoryResultCode.Ok,
                    Items = [document.ToDomain()],
                };
            }
            catch
            {
                return new MongoRepositoryResult
                {
                    Code = (int)MongoRepositoryResultCode.InternalServerError,
                };
            }
        }

        public async Task<MongoRepositoryResult> CreateAsync(Item item)
        {
            //log           
            Console.WriteLine($"***{nameof(MongoRepository)} is running a create operation.");
            try
            {
                var document = new ItemDocument
                {
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                };
                await _context.Documents.InsertOneAsync(document);
                return new MongoRepositoryResult
                {
                    Code = (int)MongoRepositoryResultCode.Created,
                    Items = [document.ToDomain()],
                };
            }
            catch
            {
                return new MongoRepositoryResult
                {
                    Code = (int)MongoRepositoryResultCode.InternalServerError,
                };
            }
        }

        public async Task<MongoRepositoryResult> UpdateAsync(Item item)
        {
            //log           
            Console.WriteLine($"***{nameof(MongoRepository)} is running an update operation.");
            try
            {
                if (!IdValidation(item.Id))
                    return new MongoRepositoryResult
                    {
                        Code = (int)MongoRepositoryResultCode.BadRequest,
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
                    return new MongoRepositoryResult
                    {
                        Code = (int)MongoRepositoryResultCode.NotFound
                    };
                return new MongoRepositoryResult
                {
                    Code = (int)MongoRepositoryResultCode.Ok,
                    Items = [document.ToDomain()],
                };
            }
            catch
            {
                return new MongoRepositoryResult
                {
                    Code = (int)MongoRepositoryResultCode.InternalServerError,
                };
            }
        }

        public async Task<MongoRepositoryResult> DeleteAsync(string id)
        {
            //log           
            Console.WriteLine($"***{nameof(MongoRepository)} is running a delete operation.");
            try
            {
                if (!IdValidation(id))
                    return new MongoRepositoryResult
                    {
                        Code = (int)MongoRepositoryResultCode.BadRequest,
                    };
                var result = await _context.Documents.DeleteOneAsync(
                    filter.Eq(p => p.Id, ObjectId.Parse(id)));
                if (result.DeletedCount == 0)
                    return new MongoRepositoryResult
                    {
                        Code = (int)MongoRepositoryResultCode.NotFound
                    };
                return new MongoRepositoryResult
                {
                    Code = (int)MongoRepositoryResultCode.NoContent,
                };
            }
            catch
            {
                return new MongoRepositoryResult
                {
                    Code = (int)MongoRepositoryResultCode.InternalServerError,
                };
            }
        }                 
        
    }
}