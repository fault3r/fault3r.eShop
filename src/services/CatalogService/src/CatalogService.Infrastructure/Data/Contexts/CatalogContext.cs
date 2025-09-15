
using System;
using CatalogService.Infrastructure.Data.Documents;
using MongoDB.Driver;

namespace CatalogService.Infrastructure.Data.Contexts
{
    public class CatalogContext
    {
        public readonly IMongoCollection<ItemDocument> Documents;

        public CatalogContext(MongoClient client, string DatabaseName, string CollectionName)
        {
            var database = client.GetDatabase(DatabaseName);
            Documents = database.GetCollection<ItemDocument>(CollectionName);
        }
    }
}