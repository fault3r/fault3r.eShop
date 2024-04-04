using fault3r_Presentation.Models.ViewModels.Account;
using FluentValidation;
using System.IO;

namespace fault3r_Presentation.Models.Validators.Account
{
    public class UpdateAccountValidator: AbstractValidator<UpdateAccountViewModel>
    {
        public UpdateAccountValidator()
        {
            RuleFor(p=>p.Name)
                .NotEmpty().WithMessage("نام را وارد کنید.")
                .Length(3, 30).WithMessage("نام باید بین 3 تا 30 حرف باشد.");

            RuleFor(p => p)
                .Must(p => p.Picture != null ? p.Picture.FileName.ToLower().Contains(".jpg") || p.Picture.FileName.ToLower().Contains(".png") : true)
                .WithMessage("فایل تصویر نامعتبر است.");

            RuleFor(p => p.Bio)
                .Length(0, 150).WithMessage("درباره باید حداکثر 150 حرف باشد.");
        }
    }
}
