

using fault3r_Application.Interfaces;
using fault3r_Application.Services.AccountsRepository.Dtos;
using fault3r_Application.Services.LoggingService;
using fault3r_Common;
using fault3r_Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace fault3r_Application.Services.AccountsRepository
{
    public class AccountsRepository: IAccountsRepository
    {        
        private readonly IDatabaseContext _databaseContext;
        private readonly IHttpContextAccessor _httpcontextAccessor;
        private readonly ILoggingService _loggingService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountsRepository(IDatabaseContext databaseContext, IHttpContextAccessor httpcontextAccessor,
            ILoggingService loggingService, IWebHostEnvironment webHostEnvironment)
        {
            _databaseContext = databaseContext;
            _httpcontextAccessor = httpcontextAccessor;
            _loggingService = loggingService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<AccountsRepositoryResult> SignUpAsync(SignUpDto account)
        {
            Account newAccount = new();
            try
            {
                newAccount = new Account
                {
                    Email = account.Email.ToLower().Trim(),
                    Password = PasswordHasher.ToHash(account.Password),
                    RoleId = Guid.Parse(AppRoles.ACCOUNT),
                    Name = account.Name.Trim(),
                    Picture = ResourceMemorizer.DefaultProfilePicture.ToArray(),
                    RankId = _databaseContext.Ranks.Where(p => p.RankNumber == 1).Select(r => r.Id).FirstOrDefault(),
                    Date = DateTime.Now.ToString("yyyy/MM/dd"),
                    Bio = null,
                    IsActive = true,
                };
                _databaseContext.Accounts.Add(newAccount);
                await _databaseContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string message = string.Empty;
                if (e.InnerException != null)
                {
                    if (e.InnerException.Message.Contains("duplicate key"))
                        message = "این آدرس ایمیل قبلا در سایت ثبت شده است.";
                    else
                        message = "خطا!";
                }
                else
                    message = "خطا!";
                return new AccountsRepositoryResult { Success = false, Message = message };
            }
            await _loggingService.AddAccountLogAsync(newAccount.Email, LoggingService.AccountLogTitles.SignUp);
            return new AccountsRepositoryResult { Success = true, Message = "ثبت نام شما در سایت با موفقیت انجام شد." };
        }

        public async Task<AccountsRepositoryResult> SignInAsync(string email, string password, bool persistent)
        {
            Account account = new();
            try
            {                
                account = await _databaseContext.Accounts.AsQueryable()
                    .Include(e => e.Role)
                    .Where(p => p.Email == email.ToLower().Trim() && p.Password == PasswordHasher.ToHash(password))
                    .FirstOrDefaultAsync();
            }
            catch
            {
                return new AccountsRepositoryResult { Success = false, Message = "خطا!" };
            }
            if (account != null)
            {
                if (account.IsActive)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                        new Claim(ClaimTypes.Email,account.Email),
                        new Claim(ClaimTypes.Role,account.Role.Name),
                        new Claim(ClaimTypes.Name,account.Name),
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties() { IsPersistent = persistent };
                    await _httpcontextAccessor.HttpContext.SignInAsync(principal, properties);
                }
                else
                    return new AccountsRepositoryResult { Success = false, Message = "حساب کاربری شما غیر فعال است." };
            }
            else
                return new AccountsRepositoryResult { Success = false, Message = "ایمیل یا کلمه عبور اشتباه است." };
            await _loggingService.AddAccountLogAsync(account.Email, LoggingService.AccountLogTitles.SignIn);            
            return new AccountsRepositoryResult { Success = true, Message = "ورود شما به سایت با موفقیت انجام شد." };
        }

        public async Task<AccountsRepositoryResult> SignOutAsync(string email)
        {               
            await _httpcontextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _loggingService.AddAccountLogAsync(email, LoggingService.AccountLogTitles.SignOut);
            return new AccountsRepositoryResult { Success = true, Message = "خروج شما از سایت با موفقیت انجام شد." };
        }    
    }    
}
