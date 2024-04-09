using fault3r_Application.Services.UsersRepository.Dto;
using System.Collections.Generic;

namespace fault3r_Application.Services.UsersRepository
{
    public interface IUsersRepository
    {
        UsersDto GetUsers(string search, int page);

        List<RankDto> GetRanks();
    }
}
