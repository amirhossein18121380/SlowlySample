using FluentValidation;
using SlowlySimulate.Api.ViewModels.Account;

namespace SlowlySimulate.Api.ViewModels.Account.Validators
{
    public class ChangePasswordViewModelValidator<T> : AbstractValidator<T> where T : ChangePasswordViewModel
    {
        public ChangePasswordViewModelValidator()
        {
            RuleFor(p => p.UserId)
                .NotEmpty();

            RuleFor(p => p.Password).Password().WithName("NewPassword");

            RuleFor(p => p.PasswordConfirm)
                .Equal(p => p.Password).WithMessage(x => "PasswordConfirmationFailed").WithName("ConfirmNewPassword");
        }
    }

    public class ChangePasswordViewModelValidator : ChangePasswordViewModelValidator<ChangePasswordViewModel>
    {
        public ChangePasswordViewModelValidator()
        {

        }
    }
}
