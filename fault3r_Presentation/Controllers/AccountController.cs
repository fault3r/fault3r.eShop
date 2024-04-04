using fault3r_Application.Services.AccountRepository;
using fault3r_Application.Services.AccountRepository.Dtos;
using fault3r_Common;
using fault3r_Presentation.Models.ViewModels.Account;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace fault3r_Presentation.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IValidator<UpdateAccountViewModel> _updateAccountValidator;
        private readonly IValidator<ChangePasswordAccountViewModel> _changePasswordAccountValidator;
        private readonly IValidator<DeleteAccountViewModel> _deleteAccountValidator;


        public AccountController(IAccountRepository accountRepository,
            IValidator<UpdateAccountViewModel> updateAccountValidator, IValidator<ChangePasswordAccountViewModel> changePasswordAccountValidator, IValidator<DeleteAccountViewModel> deleteAccountValidator)
        {
            _accountRepository = accountRepository;
            _updateAccountValidator = updateAccountValidator;
            _changePasswordAccountValidator = changePasswordAccountValidator;
            _deleteAccountValidator = deleteAccountValidator;
        }

        [Route("Index")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tAccount = await _accountRepository.ViewAccountAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
            if (tAccount == null)
                return Redirect("/Accounts/SignOut");
            var account = new ViewAccountViewModel
            {
                Email = tAccount.Email,
                Name = tAccount.Name,
                Picture = tAccount.Picture,
                Rank = tAccount.Rank,
                Date = tAccount.Date,
                Bio = tAccount.Bio,
            };
            return View("Index", account);
        }

        [Route("Update")]
        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var tAccount = await _accountRepository.ViewAccountAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
            if (tAccount == null)
                return Redirect("/Accounts/SignOut");
            var account = new UpdateAccountViewModel
            {
                Name = tAccount.Name,
                Picture = null,
                Bio = tAccount.Bio,
            };
            return View("Update", account);
        }

        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update([FromForm] UpdateAccountViewModel account)
        {
            var vResult = _updateAccountValidator.Validate(account);
            if (vResult.IsValid)
            {
                var result = await _accountRepository.UpdateAccountAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString(), new UpdateAccountDto
                {
                    Name = account.Name,
                    Picture = account.Picture,
                    Bio = account.Bio,
                });
                ModelState.AddModelError("Update", result.Message);
            }
            else
                ModelState.AddFluentResult(vResult);
            return View("Update", account);
        }

        [Route("ChangePassword")]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View("ChangePassword");
        }

        [Route("ChangePassword")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordAccountViewModel account)
        {
            var vResult = _changePasswordAccountValidator.Validate(account);
            if (vResult.IsValid)
            {
                var result = await _accountRepository.ChangePasswordAccountAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString(), new ChangePasswordAccountDto
                {
                    CurrentPassword = account.CurrentPassword,
                    NewPassword = account.NewPassword,
                });
                ModelState.AddModelError("ChangePassword", result.Message);
            }
            else
                ModelState.AddFluentResult(vResult);
            return View("ChangePassword", account);
        }

        [Route("Delete")]
        [HttpGet]
        public IActionResult Delete()
        {
            return View("Delete");
        }

        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] DeleteAccountViewModel account)
        {
            var vResult = _deleteAccountValidator.Validate(account);
            if (vResult.IsValid)
            {
                var result = await _accountRepository.DeleteAccountAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
                if (result.Success)
                    return Redirect("/Accounts/SignOut");
                else
                    ModelState.AddModelError("Delete", result.Message);
            }
            else
                ModelState.AddFluentResult(vResult);
            return View("Delete", account);
        }
    }
}
