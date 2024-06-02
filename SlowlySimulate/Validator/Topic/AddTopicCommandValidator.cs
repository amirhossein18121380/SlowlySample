using Application.Topic.Commands;
using FluentValidation;

namespace SlowlySimulate.Validator.Topic;
public class AddTopicCommandValidator : AbstractValidator<AddTopicCommand>
{
    public AddTopicCommandValidator()
    {
        RuleFor(x => x.TopicName).NotNull().WithMessage("Topic Name is required!");
    }
}