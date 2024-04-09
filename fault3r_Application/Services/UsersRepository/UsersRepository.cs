
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

        public List<RankDto> GetRanks()
        {
            var ranks = _databaseContext.Ranks.AsQueryable()
                .Include(e => e.Forum)
                .OrderBy(p => p.RankNumber)
                .Select(r => new RankDto
                {
                    RankNumber = r.RankNumber,
                    RankName = r.RankName,
                    IsAdmin = r.ForumId != null ? "بلی" : "خیر",
                    ForumName = r.ForumId != null ? r.Forum.Title : "---",
                })
                .ToList();
            return ranks;
        }
        
    }
}
