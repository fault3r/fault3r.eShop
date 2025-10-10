
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogManagementService.Infrastructure.Data.Documents.BaseDocument
{
    public abstract class ItemBaseDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("Id")]
        public ObjectId Id { get; set; }
    }
}