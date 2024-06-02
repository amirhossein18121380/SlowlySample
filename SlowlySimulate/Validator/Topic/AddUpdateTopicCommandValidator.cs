using Application.Topic.Commands;
using FluentValidation;

namespace SlowlySimulate.Validator.Topic;

public class AddUpdateTopicCommandValidator : AbstractValidator<AddUpdateTopicCommand>
{
    public AddUpdateTopicCommandValidator()
    {
        RuleFor(x => x.TopicName).NotNull().WithMessage("Topic Name is required!");
    }
}