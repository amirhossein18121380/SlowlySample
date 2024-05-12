using Application.Common.Commands;
using Application.Common.Services;
using CrossCuttingConcerns.Models;
using Domain.Resources.Topic;

namespace Application.Topic.Commands;

public class DeleteTopicCommand : ICommand<ApiResponse>
{
    public Guid TopicId { get; set; }
}

internal class DeleteTopicCommandHandler : ICommandHandler<DeleteTopicCommand, ApiResponse>
{
    private readonly ICrudService<Domain.Models.Topic> _topicService;

    public DeleteTopicCommandHandler(ICrudService<Domain.Models.Topic> topicService)
    {
        _topicService = topicService;
    }

    public async Task<ApiResponse> HandleAsync(DeleteTopicCommand command, CancellationToken cancellationToken = default)
    {
        var topic = await _topicService.GetByIdAsync(command.TopicId, cancellationToken);
        if (topic == null)
        {
            return new ApiResponse(statusCode: 404, TopicR.Topic_Not_Exist);
        }

        await _topicService.DeleteAsync(topic, cancellationToken);
        return new ApiResponse(statusCode: 200);
    }
}