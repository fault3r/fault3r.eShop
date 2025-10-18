
using System;
using CatalogManagementService.Application.DTOs;
using MongoDB.Bson;

namespace CatalogManagementService.Application.UseCases.UpdateItem
{
    public class UpdateItemValidator
    {
        public static bool IsValid(string id, UpdateItemDto item)
        {
            if (
                !ObjectId.TryParse(id, out var bId) ||
                string.IsNullOrEmpty(item.Name) ||
                string.IsNullOrEmpty(item.Description) ||
                item.Price < 1)
            {
                return false;
            }
            else
                return true;
        }
    }
}
