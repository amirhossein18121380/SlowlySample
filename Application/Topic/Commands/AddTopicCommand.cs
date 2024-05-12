using Application.Common.Commands;
using Application.Topic.Services;
using CrossCuttingConcerns.Models;
using System.ComponentModel.DataAnnotations;

namespace Application.Topic.Commands;

public class AddTopicCommand : ICommand<ApiResponse>
{
    [Required]
    public string TopicName { get; set; }
}

internal class AddTopicCommandHandler : ICommandHandler<AddTopicCommand, ApiResponse>
{
    private readonly ITopicService _topicService;

    public AddTopicCommandHandler(ITopicService topicService)
    {
        _topicService = topicService;
    }

    public async Task<ApiResponse> HandleAsync(AddTopicCommand command, CancellationToken cancellationToken = default)
    {
        var topic = new Domain.Models.Topic()
        {
            Name = command.TopicName
        };
        await _topicService.AddAsync(topic, cancellationToken);
        return new ApiResponse(statusCode: 200);
    }
}