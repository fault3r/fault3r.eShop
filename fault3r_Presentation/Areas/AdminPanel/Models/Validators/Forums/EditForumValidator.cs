using fault3r_Presentation.Areas.AdminPanel.Models.ViewModels.Forums;
using FluentValidation;

namespace fault3r_Presentation.Areas.AdminPanel.Models.Validators.Forums
{
    public class EditForumValidator:AbstractValidator<EditForumViewModel>
    {
        public EditForumValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("عنوان را وارد کنید.")
                .Length(3, 50).WithMessage("عنوان باید بین 3 تا 50 حرف باشد.");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("شرح را وارد کنید.")
                .Length(3, 200).WithMessage("شرح باید بین 3 تا 200 حرف باشد.");

            RuleFor(p => p)
              .Must(p=>p.Image != null ? p.Image.FileName.ToLower().Contains(".jpg") || p.Image.FileName.ToLower().Contains(".png") : true)
              .WithMessage("فایل تصویر نامعتبر است.");
        }
    }
}
