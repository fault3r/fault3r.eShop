using fault3r_Presentation.Models.ViewModels.Account;
using FluentValidation;

namespace fault3r_Presentation.Models.Validators.Account
{
    public class DeleteAccountValidator:AbstractValidator<DeleteAccountViewModel>
    {
        public DeleteAccountValidator()
        {
            RuleFor(p => p)
                .Must(p => p.DeleteConfirm)
                .WithMessage("با حذف حساب کاربری موافقت کنید.");
        }
    }
}
