
using System;
using CatalogManagementService.Infrastructure.Data.Documents;
using MongoDB.Driver;

namespace CatalogManagementService.Infrastructure.Data.Contexts
{
    public class CatalogManagementContext
    {
        public readonly IMongoCollection<ItemDocument> Documents;

        public CatalogManagementContext(MongoClient client, string DatabaseName, string CollectionName)
        {
            var database = client.GetDatabase(DatabaseName);
            Documents = database.GetCollection<ItemDocument>(CollectionName);
        }
    }
}