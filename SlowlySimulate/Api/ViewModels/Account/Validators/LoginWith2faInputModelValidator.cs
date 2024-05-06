using FluentValidation;
using SlowlySimulate.Api.ViewModels.Account;

namespace SlowlySimulate.Api.ViewModels.Account.Validators
{
    public class LoginWith2faInputModelValidator : AbstractValidator<LoginWith2faInputModel>
    {
        public LoginWith2faInputModelValidator()
        {
            RuleFor(p => p.TwoFactorCode)
                .NotEmpty()
                .Length(6, 7).WithName("AuthenticatorCode");
        }
    }
}
