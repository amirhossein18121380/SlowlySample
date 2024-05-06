using FluentValidation;
using SlowlySimulate.Api.ViewModels.Account;

namespace SlowlySimulate.Api.ViewModels.Account.Validators
{
    public class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        public UserViewModelValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty()
                .Matches(@"^[^\s]+$").WithMessage(x => "SpacesNotPermitted")
                .Length(2, 64).WithName("UserName");

            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress().WithName("Email");
        }
    }
}
