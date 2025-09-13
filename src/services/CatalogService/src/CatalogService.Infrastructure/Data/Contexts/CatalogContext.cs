
using System;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CatalogService.Infrastructure.Data.Contexts
{
    public class CatalogContext
    {
        public readonly IMongoCollection<Item> Collection;

        public CatalogContext(IOptions<MongodbSettings> mongodbSettings)
        {
            var settings = mongodbSettings.Value;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            Collection = database.GetCollection<Item>(settings.CollectionName);
        }
    }
}