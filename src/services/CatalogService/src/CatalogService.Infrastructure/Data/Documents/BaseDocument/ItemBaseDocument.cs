using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogService.Infrastructure.Data.Documents.BaseDocument
{
    public class ItemBaseDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("Id")]
        public ObjectId Id { get; set; }
    }
}