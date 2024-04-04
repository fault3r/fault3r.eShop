
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace fault3r_Common
{
    public static class Extensions
    {
        public static ModelStateDictionary AddFluentResult(this ModelStateDictionary source, ValidationResult fluentResult)
        {
            foreach (var error in fluentResult.Errors)
                source.AddModelError(error.PropertyName, error.ErrorMessage);
            return source;
        }

        public static IEnumerable<Entity> ToPagination<Entity>(this IEnumerable<Entity> source, int page, out PaginationDto pagination)
        {                                                                        //template: [<] [1] ... [3][4][-5-][6][7] ... [10] [>] 
            pagination = new();                      
            pagination.PageCount = (int)Math.Ceiling((double)source.Count() / PaginationDto.PageSize);
            page = page < 1 ? 1 : page;
            pagination.CurrentPage = page > pagination.PageCount ? pagination.PageCount : page;           
            for (int i = pagination.CurrentPage - 2; i <= pagination.CurrentPage + 2; i++)            
                if (i > 0 && i <= pagination.PageCount)
                    pagination.Pages.Add(i);            
            if (pagination.CurrentPage > 1)
                pagination.HasPrev = true;
            if ((pagination.CurrentPage - 3) > 0)
                pagination.HasMorePrev = true;
            if (pagination.CurrentPage < pagination.PageCount)
                pagination.HasNext = true;
            if ((pagination.CurrentPage + 3) <= pagination.PageCount)
                pagination.HasMoreNext = true;
            return source.Skip((pagination.CurrentPage - 1) * PaginationDto.PageSize)
                         .Take(PaginationDto.PageSize); 
        }
    }
}
