using System;
using CatalogManagementService.Application.DTOs;
using MediatR;

namespace CatalogManagementService.Application.MediatR.Queries
{
    public class GetAllQuery : IRequest<(int Code, IEnumerable<ItemDto> Items)> { }
}