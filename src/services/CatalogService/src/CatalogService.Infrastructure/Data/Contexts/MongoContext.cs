
using System;
using CatalogService.Application.Interfaces;
using CatalogService.Infrastructure.Data.Documents;
using MongoDB.Driver;

namespace CatalogService.Infrastructure.Data.Contexts
{
    public class MongoContext
    {
        public readonly IMongoDatabase Database;

        public readonly IMongoCollection<ItemDocument> Documents;

        private readonly ILoggerService<MongoContext> _logger;

        public MongoContext(MongoClient client, string DatabaseName, string CollectionName,
            ILoggerService<MongoContext> logger)
        {
            Database = client.GetDatabase(DatabaseName);
            Documents = Database.GetCollection<ItemDocument>(CollectionName);
            _logger = logger;
            _logger.LogInformation("instance created.");
        }
    }
}