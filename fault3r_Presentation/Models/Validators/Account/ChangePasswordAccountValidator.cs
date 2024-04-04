using fault3r_Presentation.Models.ViewModels.Account;
using FluentValidation;

namespace fault3r_Presentation.Models.Validators.Account
{
    public class ChangePasswordAccountValidator:AbstractValidator<ChangePasswordAccountViewModel>
    {
        public ChangePasswordAccountValidator()
        {

            RuleFor(p => p.CurrentPassword)
                .NotEmpty().WithMessage("کلمه عبور فعلی را وارد کنید.")
                .Length(6, 20).WithMessage("کلمه عبور فعلی باید بین 6 تا 20 حرف باشد.");

            RuleFor(p => p.NewPassword)
                .NotEmpty().WithMessage("کلمه عبور جدید را وارد کنید.")
                .Length(6, 20).WithMessage("کلمه عبور جدید باید بین 6 تا 20 حرف باشد.");

            RuleFor(p => p.ConfirmPassword)
                .NotEmpty().WithMessage("تکرار کلمه عبور را وارد کنید.")
                .Length(6, 20).WithMessage("تکرار کلمه عبور باید بین 6 تا 20 حرف باشد.");

            RuleFor(p => p)
                .Must(p=>p.NewPassword == p.ConfirmPassword)
                .WithMessage("کلمه عبور جدید و تکرار آن یکی نیست.");
        }
    }
}
