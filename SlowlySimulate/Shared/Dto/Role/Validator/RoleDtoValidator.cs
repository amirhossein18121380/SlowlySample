using FluentValidation;
using SlowlySimulate.Shared.Dto.Role;

namespace SlowlySimulate.Shared.Dto.Role.Validator;
public class RoleDtoValidator : AbstractValidator<RoleDto>
{
    public RoleDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty();
        RuleFor(x => x.Permissions)
            .NotEmpty();
    }
}