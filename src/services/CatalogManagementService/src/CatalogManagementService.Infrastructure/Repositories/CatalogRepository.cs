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
    public class CatalogRepository : ICatalogRepository
    {
        private readonly CatalogContext _context;

        private readonly FilterDefinitionBuilder<ItemDocument> filter;

        public CatalogRepository(CatalogContext context)
        {
            _context = context;
            filter = Builders<ItemDocument>.Filter;
        }

        public async Task<RepositoryResult> GetAllAsync()
        {
            try
            {
                var documents = await _context.Documents.Find(filter.Empty)
                    .ToListAsync();
                return new RepositoryResult
                {
                    Success = true,
                    Message = "success.",
                    Items = documents.Select(document => document.ToDomain()),
                };
            }
            catch (Exception ex)
            {
                return new RepositoryResult { Message = ex.Message };
            }
        }

        public async Task<RepositoryResult> GetByIdAsync(string id)
        {
            try
            {
                var document = await _context.Documents.Find(filter.Eq(p => p.Id, ObjectId.Parse(id)))
                    .FirstOrDefaultAsync();
                if (document is null)
                    return new RepositoryResult { Message = "not found!" };
                return new RepositoryResult
                {
                    Success = true,
                    Message = "success.",
                    Items = [document.ToDomain()],
                };
            }
            catch (Exception ex)
            {
                return new RepositoryResult { Message = ex.Message };
            }
        }

        public async Task<RepositoryResult> CreateAsync(Item item)
        {
            try
            {
                var document = new ItemDocument
                {
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Pictures = item.Pictures,
                };
                await _context.Documents.InsertOneAsync(document);
                return new RepositoryResult
                {
                    Success = true,
                    Message = "success.",
                    Items = [document.ToDomain()],
                };
            }
            catch (Exception ex)
            {
                return new RepositoryResult { Message = ex.Message };
            }
        }

        public async Task<RepositoryResult> UpdateAsync(Item item)
        {
            try
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
                    return new RepositoryResult { Message = "not found!" };
                return new RepositoryResult
                {
                    Success = true,
                    Message = "success",
                    Items = [document.ToDomain()],
                };
            }
            catch (Exception ex)
            {
                return new RepositoryResult { Message = ex.Message };
            }
        }

        public async Task<RepositoryResult> DeleteAsync(string id)
        {
            try
            {
                var result = await _context.Documents.DeleteOneAsync(
                    filter.Eq(p => p.Id, ObjectId.Parse(id)));
                if (result.DeletedCount == 0)
                    return new RepositoryResult { Message = "not found!" };
                return new RepositoryResult
                {
                    Success = true,
                    Message = "success",
                };
            }
            catch (Exception ex)
            {
                return new RepositoryResult { Message = ex.Message };
            }
        }
        
    }
}