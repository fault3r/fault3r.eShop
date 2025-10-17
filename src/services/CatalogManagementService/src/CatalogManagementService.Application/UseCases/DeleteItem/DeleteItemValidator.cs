
using System;
using MongoDB.Bson;

namespace CatalogManagementService.Application.UseCases.DeleteItem
{
    public class DeleteItemValidator
    {
        public static bool IsValid(string id) =>
            ObjectId.TryParse(id, out var bId);
    }
}