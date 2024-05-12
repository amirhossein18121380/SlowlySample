using FluentValidation;

namespace Domain.Dto.Email.Validators
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
