using Application.Topic.Services;
using CrossCuttingConcerns.Models;
using MediatR;

namespace Application.Topic.Commands;

public class AddTopicCommand : IRequest<ApiResponse>//: ICommand<ApiResponse>
{
    public string TopicName { get; set; }
}

internal class AddTopicCommandHandler : IRequestHandler<AddTopicCommand, ApiResponse>
{
    private readonly ITopicService _topicService;

    public AddTopicCommandHandler(ITopicService topicService)
    {
        _topicService = topicService;
    }

    public async Task<ApiResponse> Handle(AddTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = new Domain.Models.Topic()
        {
            Name = request.TopicName
        };
        await _topicService.AddAsync(topic, cancellationToken);
        return new ApiResponse(statusCode: 200);
    }

    //public async Task<ApiResponse> HandleAsync(AddTopicCommand command, CancellationToken cancellationToken = default)
    //{
    //    var topic = new Domain.Models.Topic()
    //    {
    //        Name = command.TopicName
    //    };
    //    await _topicService.AddAsync(topic, cancellationToken);
    //    return new ApiResponse(statusCode: 200);
    //}
}