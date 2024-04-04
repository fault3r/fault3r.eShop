using fault3r_Application.Services.AccountsRepository;
using fault3r_Application.Services.AccountsRepository.Dtos;
using fault3r_Common;
using fault3r_Presentation.Models.ViewModels.Accounts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Threading.Tasks;

namespace fault3r_Presentation.Controllers
{
    [Route("[controller]")]
    public class AccountsController : Controller
    {
        private readonly IAccountsRepository _accountsRepository;
        private readonly IValidator<SignUpViewModel> _signupValidator;
        private readonly IValidator<SignInViewModel> _signinValidator;

        public AccountsController(IAccountsRepository accountsRepository,
            IValidator<SignUpViewModel> signupValidator, IValidator<SignInViewModel> signinValidator)
        {
            _accountsRepository = accountsRepository;
            _signupValidator = signupValidator;
            _signinValidator = signinValidator;
        }
      
        [Route("SignUp")]
        [HttpGet]
        public IActionResult SignUp()
        {
            return View("SignUp");
        }

        [Route("SignUp")]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromForm]SignUpViewModel account)
        {
            var validateResult = _signupValidator.Validate(account);
            if (validateResult.IsValid)
            {                    
                var result = await _accountsRepository.SignUpAsync(new SignUpDto
                {
                    Email = account.Email,
                    Password = account.Password,
                    Name = account.Name,
                });
                ModelState.AddModelError("SignUp", result.Message);
            }
            else
                ModelState.AddFluentResult(validateResult);

            return View("SignUp", account);
        }

        [Route("SignIn")]
        [HttpGet]
        public IActionResult SignIn([FromQuery]string ReturnUrl)
        {
            if (ReturnUrl != null)
                ModelState.AddModelError("SignIn", "برای دیدن این صفحه ابتدا باید وارد سایت شوید.");
            return View("SignIn");
        }

        [Route("SignIn")]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromForm]SignInViewModel account)
        {
            var validateResult = _signinValidator.Validate(account);
            if (validateResult.IsValid)
            {
                var result = await _accountsRepository.SignInAsync(account.Email, account.Password, account.Persistent);
                if (result.Success)         
                    return Redirect("/Account/Index");                
                else
                    ModelState.AddModelError("SignIn", result.Message);
            }
            else
                ModelState.AddFluentResult(validateResult);
            return View("SignIn", account);
        }

        [Route("SignOut")]
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            if (User.Identity.IsAuthenticated)            
                await _accountsRepository.SignOutAsync(User.FindFirst(ClaimTypes.Email).Value.ToString());
            return Redirect("/Forums/Index");
        }

    }
}
