using SlowlySimulate.Application.Common.Commands;
using SlowlySimulate.Application.Common.Services;
using SlowlySimulate.CrossCuttingConcerns.Models;
using SlowlySimulate.Domain.Repositories;
using SlowlySimulate.Shared.Resources.Topic;

namespace SlowlySimulate.Application.Topic.Commands;

public class AddUpdateTopicCommand : ICommand<ApiResponse>
{
    public required String TopicName { get; set; }
    public Guid TopicId { get; set; }
}

internal class AddUpdateTopicCommandHandler : ICommandHandler<AddUpdateTopicCommand, ApiResponse>
{
    private readonly ICrudService<Domain.Models.Topic> _topicService;
    private readonly IRepository<Domain.Models.Topic, Guid> _topicRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddUpdateTopicCommandHandler(ICrudService<Domain.Models.Topic> topicService, IRepository<Domain.Models.Topic, Guid> topicRepository, IUnitOfWork unitOfWork)
    {
        _topicService = topicService;
        _topicRepository = topicRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse> HandleAsync(AddUpdateTopicCommand command, CancellationToken cancellationToken = default)
    {
        var existingTopic = _topicRepository.GetQueryableSet().SingleOrDefault(x => x.Id == command.TopicId);

        if (existingTopic == null && command.TopicId != Guid.Empty)
        {
            return new ApiResponse(statusCode: 404, TopicR.Topic_Not_Exist);
        }

        var topic = existingTopic ?? new Domain.Models.Topic();

        // Always update the name, regardless of whether it's a new or existing topic
        topic.Name = command.TopicName;


        using (await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken))
        {
            await _topicService.AddOrUpdateAsync(topic, cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }

        return new ApiResponse(statusCode: 200);
    }
}
