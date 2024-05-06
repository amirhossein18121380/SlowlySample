using FluentValidation;
using SlowlySimulate.Api.ViewModels.Account;

namespace SlowlySimulate.Api.ViewModels.Account.Validators
{
    public class ForgotPasswordViewModelValidator : AbstractValidator<ForgotPasswordViewModel>
    {
        public ForgotPasswordViewModelValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress().WithName("Email");
        }
    }
}
