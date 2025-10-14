
using System;
using CatalogManagementService.Infrastructure.Data.Documents;
using MongoDB.Driver;

namespace CatalogManagementService.Infrastructure.Data.Contexts
{
    public class MongoContext
    {
        public readonly IMongoDatabase Database;

        public readonly IMongoCollection<ItemDocument> Documents;

        public MongoContext(MongoClient client, string DatabaseName, string CollectionName)
        {
            //log
            Console.WriteLine($"***{nameof(MongoContext)} is being configured.");
            Database = client.GetDatabase(DatabaseName);
            Documents = Database.GetCollection<ItemDocument>(CollectionName);
        }
        
    }
}