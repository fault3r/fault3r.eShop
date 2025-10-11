
using System;
using CatalogManagementService.Domain.DTOs;
using CatalogManagementService.Domain.Entities;

namespace CatalogManagementService.Domain.Interfaces
{
    public interface IRepository
    {
        Task<MongoRepositoryResult> GetAllAsync();

        Task<MongoRepositoryResult> GetByIdAsync(string id);

        Task<MongoRepositoryResult> CreateAsync(Item item);

        Task<MongoRepositoryResult> UpdateAsync(Item item);

        Task<MongoRepositoryResult> DeleteAsync(string id);
        
    }
}