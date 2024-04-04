using fault3r_Presentation.Models.ViewModels.Accounts;
using FluentValidation;

namespace fault3r_Presentation.Models.Validators.Accounts
{
    public class SignUpValidator: AbstractValidator<SignUpViewModel>
    {
        public SignUpValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("ایمیل را وارد کنید.")
                .EmailAddress().WithMessage("ایمیل نادرست است.");

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("کلمه عبور را وارد کنید.")
                .Length(6, 20).WithMessage("کلمه عبور باید بین 6 تا 20 حرف باشد.");

            RuleFor(p => p.RePassword)
                .NotEmpty().WithMessage("تکرار کلمه عبور را وارد کنید.")
                .Length(6, 20).WithMessage("تکرار کلمه عبور باید بین 6 تا 20 حرف باشد.");

            RuleFor(p => p)
                .Must(p => p.Password == p.RePassword)
                    .WithMessage("کلمه عبور و تکرار آن یکی نیست.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("نام را وارد کنید.")
                .Length(3, 30).WithMessage("نام باید بین 3 تا 30 حرف باشد.");

            RuleFor(p => p)
                .Must(p => p.Terms)
                .WithMessage("با قوانین سایت موافقت کنید.");
        }
    }
}
