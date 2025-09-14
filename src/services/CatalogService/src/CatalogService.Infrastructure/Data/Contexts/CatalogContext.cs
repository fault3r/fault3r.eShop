
using System;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Configurations;
using MongoDB.Driver;

namespace CatalogService.Infrastructure.Data.Contexts
{
    public class CatalogContext
    {
        public readonly IMongoCollection<Item> Items;

        public CatalogContext(MongodbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            Items = database.GetCollection<Item>(settings.CollectionName);
        }
    }
}