using fault3r_Presentation.Areas.AdminPanel.Models.ViewModels.Forums;
using FluentValidation;

namespace fault3r_Presentation.Areas.AdminPanel.Models.Validators.Forums
{
    public class DeleteForumValidator:AbstractValidator<DeleteForumViewModel>
    {
        public DeleteForumValidator()
        {
            RuleFor(p => p)
                .Must(p => p.Confirm).WithMessage("با حذف انجمن موافقت کنید.");
        }
    }
}
