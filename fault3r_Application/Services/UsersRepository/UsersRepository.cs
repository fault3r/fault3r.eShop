
using fault3r_Application.Interfaces;
using fault3r_Application.Services.UsersRepository.Dto;
using fault3r_Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace fault3r_Application.Services.UsersRepository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public UsersRepository(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public UsersDto GetUsers(string search, int page)
        {
            var searchUsers = _databaseContext.Accounts.AsQueryable();
            if (search != "")
                searchUsers = _databaseContext.Accounts.AsQueryable()
                    .Where(p => p.Email.Contains(search.ToLower().Trim()));
            var users = searchUsers.Include(e => e.Role).Include(e => e.Rank)
                .ToPagination(page, out PaginationDto pagination)
                .Select(r => new UserDto
                {
                    Email = r.Email,
                    Role = r.Role.Name,
                    Name = r.Name,
                    Picture = Convert.ToBase64String(r.Picture),
                    Rank = r.Rank.RankName,
                    IsActive = r.IsActive == true ? "فعال" : "غیرفعال",
                })
                .ToList();
            return new UsersDto { Users = users, Pagination = pagination };
        }
    }
}
