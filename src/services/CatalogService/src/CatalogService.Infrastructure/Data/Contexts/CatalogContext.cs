
using System;
using CatalogService.Infrastructure.Configurations;
using CatalogService.Infrastructure.Data.Documents;
using MongoDB.Driver;

namespace CatalogService.Infrastructure.Data.Contexts
{
    public class CatalogContext
    {
        public readonly IMongoCollection<ItemDocument> Items;

        public CatalogContext(ContextSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            Items = database.GetCollection<ItemDocument>(settings.CollectionName);
        }
    }
}