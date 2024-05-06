using FluentValidation;
using SlowlySimulate.Api.ViewModels.Account;

namespace SlowlySimulate.Api.ViewModels.Account.Validators
{
    public class UpdatePasswordViewModelValidator : AbstractValidator<UpdatePasswordViewModel>
    {
        public UpdatePasswordViewModelValidator()
        {
            RuleFor(p => p.NewPassword).Password().WithName("NewPassword");

            RuleFor(p => p.NewPasswordConfirm)
                .Equal(p => p.NewPassword).WithMessage(x => "PasswordConfirmationFailed").WithName("ConfirmNewPassword");

            RuleFor(p => p.CurrentPassword)
                .NotEmpty().WithName("CurrentPassword");
        }
    }
}
