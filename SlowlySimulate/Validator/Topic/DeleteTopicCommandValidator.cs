using Application.Topic.Commands;
using FluentValidation;

namespace SlowlySimulate.Validator.Topic;
public class DeleteTopicCommandValidator : AbstractValidator<DeleteTopicCommand>
{
    public DeleteTopicCommandValidator()
    {
        RuleFor(x => x.TopicId).NotNull().WithMessage("Topic Id is required!");
    }
}