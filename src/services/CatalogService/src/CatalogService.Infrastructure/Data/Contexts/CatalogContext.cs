
using System;
using CatalogService.Infrastructure.Configurations;
using CatalogService.Infrastructure.Data.Documents;
using MongoDB.Driver;

namespace CatalogService.Infrastructure.Data.Contexts
{
    public class CatalogContext
    {
        public readonly IMongoCollection<ItemDocument> Items;

        public CatalogContext(MongoClient client, string DatabaseName, string CollectionName)
        {
            var database = client.GetDatabase(DatabaseName);
            Items = database.GetCollection<ItemDocument>(CollectionName);
        }
    }
}