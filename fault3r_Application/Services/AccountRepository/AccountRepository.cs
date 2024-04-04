

using fault3r_Application.Interfaces;
using fault3r_Application.Services.AccountRepository.Dtos;
using fault3r_Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fault3r_Application.Services.AccountRepository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public AccountRepository(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ViewAccountDto> ViewAccountAsync(string id)
        {
            ViewAccountDto account = new();
            try
            {                
                account = await _databaseContext.Accounts.AsQueryable()
                    .Include(e => e.Rank)
                    .Where(p => p.Id.ToString() == id)
                    .Select(r => new ViewAccountDto
                    {
                        Email = r.Email,
                        Name = r.Name,
                        Picture = Convert.ToBase64String(r.Picture),
                        Rank = r.Rank.RankName,
                        Date = r.Date,
                        Bio = r.Bio,
                    })
                    .FirstOrDefaultAsync();
            }
            catch
            {
                return null;
            }
            return account;
        }

        public async Task<AccountRepositoryResult> UpdateAccountAsync(string id, UpdateAccountDto account)
        {
            try
            {
                var tAccount = await _databaseContext.Accounts.FirstOrDefaultAsync(p => p.Id.ToString() == id);
                tAccount.Name = account.Name;
                if (account.Picture != null)
                    tAccount.Picture = ImageResizer.ResizeImage(account.Picture, 200, 200).ToArray();
                tAccount.Bio = account.Bio;
                _databaseContext.Accounts.Update(tAccount);
                await _databaseContext.SaveChangesAsync();
            }
            catch
            {
                return new AccountRepositoryResult { Success = false, Message = "خطا!" };
            }
            return new AccountRepositoryResult { Success = true, Message = "مشخصات حساب کاربری شما با موفقیت ویرایش شد." };
        }

        public async Task<AccountRepositoryResult> ChangePasswordAccountAsync(string id, ChangePasswordAccountDto account)
        {
            try
            {
                var tAccount = await _databaseContext.Accounts.FirstOrDefaultAsync(p => p.Id.ToString() == id
                    && p.Password == PasswordHasher.ToHash(account.CurrentPassword));
                if (tAccount == null)
                    return new AccountRepositoryResult { Success = false, Message = "کلمه عبور فعلی اشتباه است." };
                tAccount.Password = PasswordHasher.ToHash(account.NewPassword);
                _databaseContext.Accounts.Update(tAccount);
                await _databaseContext.SaveChangesAsync();
            }
            catch
            {
                return new AccountRepositoryResult { Success = false, Message = "خطا!" };
            }
            return new AccountRepositoryResult { Success = true, Message = "کلمه عبور حساب کاربری شما با موفقیت تغییر کرد." };
        }

        public async Task<AccountRepositoryResult> DeleteAccountAsync(string id)
        {
            try
            {
                var tAccount = await _databaseContext.Accounts.FirstOrDefaultAsync(p => p.Id.ToString() == id);
                _databaseContext.Accounts.Remove(tAccount);
                await _databaseContext.SaveChangesAsync();
            }
            catch
            {
                return new AccountRepositoryResult { Success = false, Message = "خطا!" };
            }
            return new AccountRepositoryResult { Success = true, Message = "حساب کاربری شما با موفقیت حذف شد." };
        }
    }
}
