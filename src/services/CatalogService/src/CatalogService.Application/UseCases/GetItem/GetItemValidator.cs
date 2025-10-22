
using System;
using MongoDB.Bson;

namespace CatalogService.Application.UseCases.GetItem
{
    public class GetItemValidator
    {
        public static bool IsValid(string id) =>
            ObjectId.TryParse(id, out var bId);
    }
}