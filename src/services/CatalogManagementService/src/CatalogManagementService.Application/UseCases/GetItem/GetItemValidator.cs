
using System;
using MongoDB.Bson;

namespace CatalogManagementService.Application.UseCases.GetItem
{
    public class GetItemValidator
    {
        public static bool IsValid(string id) =>
            ObjectId.TryParse(id, out var bId);
    }
}