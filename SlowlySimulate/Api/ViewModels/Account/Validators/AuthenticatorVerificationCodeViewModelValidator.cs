using FluentValidation;
using SlowlySimulate.Api.ViewModels.Account;

namespace SlowlySimulate.Api.ViewModels.Account.Validators
{
    public class AuthenticatorVerificationCodeViewModelValidator : AbstractValidator<AuthenticatorVerificationCodeViewModel>
    {
        public AuthenticatorVerificationCodeViewModelValidator()
        {
            RuleFor(p => p.Code)
                .NotEmpty()
                .Length(6, 7).WithName("VerificationCode");
        }
    }
}
