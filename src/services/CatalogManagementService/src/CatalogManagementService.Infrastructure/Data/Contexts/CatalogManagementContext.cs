
using System;
using CatalogManagementService.Infrastructure.Data.Documents;
using MongoDB.Driver;

namespace CatalogManagementService.Infrastructure.Data.Contexts
{
    public class CatalogManagementContext
    {
        public readonly IMongoDatabase Database;

        public readonly IMongoCollection<ItemDocument> Documents;

        public CatalogManagementContext(MongoClient client, string DatabaseName, string CollectionName)
        {
            Database = client.GetDatabase(DatabaseName);
            Documents = Database.GetCollection<ItemDocument>(CollectionName);
        }
    }
}