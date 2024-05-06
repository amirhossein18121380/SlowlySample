using FluentValidation;
using SlowlySimulate.Api.ViewModels.Account;

namespace SlowlySimulate.Api.ViewModels.Account.Validators
{
    public class RegisterViewModelValidator : LoginInputModelValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress().WithName("Email");

            RuleFor(p => p.PasswordConfirm)
                .Equal(p => p.Password).WithMessage(x => "PasswordConfirmationFailed").WithName("ConfirmPassword");
        }
    }
}
