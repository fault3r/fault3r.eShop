
using System;
using CatalogManagementService.Application.DTOs;

namespace CatalogManagementService.Application.UseCases.CreateItem
{
    public class CreateItemValidator
    {
        public static bool IsValid(CreateItemDto item)
        {
            if (
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