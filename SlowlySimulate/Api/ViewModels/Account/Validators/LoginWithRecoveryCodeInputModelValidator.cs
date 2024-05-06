using FluentValidation;
using SlowlySimulate.Api.ViewModels.Account;

namespace SlowlySimulate.Api.ViewModels.Account.Validators
{
    public class LoginWithRecoveryCodeInputModelValidator : AbstractValidator<LoginWithRecoveryCodeInputModel>
    {
        public LoginWithRecoveryCodeInputModelValidator()
        {
            RuleFor(p => p.RecoveryCode)
                .NotEmpty().WithName("RecoveryCode");
        }
    }
}
