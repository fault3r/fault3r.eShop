
using System;
using CatalogService.Application.DTOs;
using MediatR;

namespace CatalogService.Application.UseCases.GetItem
{
    public class GetItemQuery : IRequest<(int Code, ItemDto? Item)>
    {
        public required string Id { get; set; }      
          
    }
}