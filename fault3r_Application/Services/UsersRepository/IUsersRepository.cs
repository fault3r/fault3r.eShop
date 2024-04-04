using fault3r_Application.Services.UsersRepository.Dto;

namespace fault3r_Application.Services.UsersRepository
{
    public interface IUsersRepository
    {
        UsersDto GetUsers(string search, int page);
    }
}
