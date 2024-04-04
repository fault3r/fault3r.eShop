using fault3r_Application.Services.AccountsRepository.Dtos;
using System.Threading.Tasks;

namespace fault3r_Application.Services.AccountsRepository
{
    public interface IAccountsRepository
    {
        Task<AccountsRepositoryResult> SignUpAsync(SignUpDto account);

        Task<AccountsRepositoryResult> SignInAsync(string email, string password, bool persistent);

        Task<AccountsRepositoryResult> SignOutAsync(string email);
    }    
}
