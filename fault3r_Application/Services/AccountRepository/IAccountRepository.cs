

using fault3r_Application.Services.AccountRepository.Dtos;
using System.Threading.Tasks;

namespace fault3r_Application.Services.AccountRepository
{
    public interface IAccountRepository
    {
        Task<ViewAccountDto> ViewAccountAsync(string id);

        Task<AccountRepositoryResult> UpdateAccountAsync(string id, UpdateAccountDto account);

        Task<AccountRepositoryResult> ChangePasswordAccountAsync(string id, ChangePasswordAccountDto account);

        Task<AccountRepositoryResult> DeleteAccountAsync(string id);        
    }
}
