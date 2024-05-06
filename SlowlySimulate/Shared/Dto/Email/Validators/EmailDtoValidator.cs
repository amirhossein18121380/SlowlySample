using FluentValidation;
using SlowlySimulate.Shared.Dto.Email;

namespace SlowlySimulate.Shared.Dto.Email.Validators
{
    public class EmailDtoValidator : AbstractValidator<EmailDto>
    {
        public EmailDtoValidator()
        {
            RuleFor(p => p.ToAddress)
                .NotEmpty()
                .EmailAddress().WithName("Email");
        }
    }
}
