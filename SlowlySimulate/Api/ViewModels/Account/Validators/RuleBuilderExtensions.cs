using FluentValidation;
using SlowlySimulate.Domain.Constants;

namespace SlowlySimulate.Api.ViewModels.Account.Validators
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder
                .NotEmpty().WithName("Password")
                .MinimumLength(PasswordPolicy.RequiredLength).WithMessage("PasswordTooShort")
                .Matches("[A-Z]").When(p => PasswordPolicy.RequireUppercase, ApplyConditionTo.CurrentValidator).WithMessage("PasswordRequiresUpper")
                .Matches("[a-z]").When(p => PasswordPolicy.RequireLowercase, ApplyConditionTo.CurrentValidator).WithMessage("PasswordRequiresLower")
                .Matches("[0-9]").When(p => PasswordPolicy.RequireDigit, ApplyConditionTo.CurrentValidator).WithMessage("PasswordRequiresDigit")
                .Matches("[^a-zA-Z0-9]").When(p => PasswordPolicy.RequireNonAlphanumeric, ApplyConditionTo.CurrentValidator).WithMessage("PasswordRequiresNonAlphanumeric");

            return options;
        }
    }
}
