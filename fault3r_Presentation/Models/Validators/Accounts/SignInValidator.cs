using fault3r_Presentation.Models.ViewModels.Accounts;
using FluentValidation;

namespace fault3r_Presentation.Models.Validators.Accounts
{
    public class SignInValidator: AbstractValidator<SignInViewModel>
    {
        public SignInValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("ایمیل را وارد کنید.")
                .EmailAddress().WithMessage("ایمیل نادرست است.");

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("کلمه عبور را وارد کنید.")
                .Length(6, 20).WithMessage("کلمه عبور باید بین 6 تا 20 حرف باشد.");
        }
    }
}
