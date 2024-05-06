using FluentValidation;
using SlowlySimulate.Api.ViewModels.Account;

namespace SlowlySimulate.Api.ViewModels.Account.Validators
{
    public class ResetPasswordViewModelValidator : ChangePasswordViewModelValidator<ResetPasswordViewModel>
    {
        public ResetPasswordViewModelValidator()
        {
            RuleFor(p => p.Token)
                .NotEmpty().WithName("RecoveryCode");
        }
    }
}
