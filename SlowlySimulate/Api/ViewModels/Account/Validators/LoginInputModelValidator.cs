using FluentValidation;
using SlowlySimulate.Api.ViewModels.Account;

namespace SlowlySimulate.Api.ViewModels.Account.Validators
{
    public class LoginInputModelValidator<T> : AbstractValidator<T> where T : LoginInputModel
    {
        public LoginInputModelValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty()
                .Matches(@"^[^\s]+$").WithMessage(x => "SpacesNotPermitted")
                .Length(2, 64).WithName("UserName");

            RuleFor(p => p.Password).Password();
        }
    }
}
