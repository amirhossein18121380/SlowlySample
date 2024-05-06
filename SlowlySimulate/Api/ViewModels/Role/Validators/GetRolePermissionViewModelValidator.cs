using FluentValidation;
using SlowlySimulate.Api.ViewModels.Role;

namespace SlowlySimulate.Api.ViewModels.Role.Validators;
public class GetRolePermissionViewModelValidator : AbstractValidator<GetRolePermissionViewModel>
{
    public GetRolePermissionViewModelValidator()
    {
        RuleFor(p => p.RoleName)
            .NotEmpty();
    }
}