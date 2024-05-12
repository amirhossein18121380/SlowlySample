using Application.Common.Commands;
using Application.TopicOfInterest.Services;
using CrossCuttingConcerns.Models;
using Domain.Identity;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.TopicOfInterest.Commands;
public class ExcludedUserTopicsCommand : ICommand<ApiResponse>
{
    public Guid UserId { get; set; }
    public List<Guid> TopicIds { get; set; }
}

internal class ExcludedUserTopicsCommandHandler : ICommandHandler<ExcludedUserTopicsCommand, ApiResponse>
{
    private readonly ITopicOfInterestService _topicOfInterestService;
    private readonly IRepository<Domain.Models.Topic, Guid> _topicRepository;
    private readonly IRepository<UserTopic, Guid> _useTopicRepository;
    private readonly ICurrentUser _currentUser;
    public ExcludedUserTopicsCommandHandler(ITopicOfInterestService topicOfInterestService, IRepository<Domain.Models.Topic, Guid> topicRepository, IRepository<UserTopic, Guid> uTRepository,
        ICurrentUser currentUser)
    {
        _topicOfInterestService = topicOfInterestService;
        _topicRepository = topicRepository;
        _useTopicRepository = uTRepository;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse> HandleAsync(ExcludedUserTopicsCommand command, CancellationToken cancellationToken = default)
    {
        var userTopicsHad = await _useTopicRepository
            .GetQueryableSet()
            .Where(x => x.UserId == _currentUser.UserId)
            .ToListAsync(cancellationToken);

        var newRowTopics = command.TopicIds.Except(userTopicsHad.Select(x => x.TopicId));
        var updateAddExcludeTopic = command.TopicIds.Except(userTopicsHad.Where(x => x.IsInterest != true & x.IsExcluded == true).Select(x => x.TopicId));
        var updateRemoveExcludeTopic = userTopicsHad.Where(x => x.IsInterest != true & x.IsExcluded == true).Select(x => x.TopicId).Except(command.TopicIds);

        foreach (var topicId in newRowTopics)
        {
            var userTopic = new Domain.Models.UserTopic
            {
                UserId = command.UserId,
                TopicId = topicId,
                IsExcluded = true,
                IsInterest = false
            };
            await _topicOfInterestService.AddAsync(userTopic, cancellationToken);
        }

        foreach (var topicId in updateAddExcludeTopic)
        {
            //getting in for each might not be best practice.but ...
            var updateTopic = await _useTopicRepository
                .GetQueryableSet()
                .FirstOrDefaultAsync(x => x.UserId == _currentUser.UserId && x.TopicId == topicId, cancellationToken);
            updateTopic.IsExcluded = true;

            if (updateTopic != null)
            {
                await _topicOfInterestService.UpdateAsync(updateTopic, cancellationToken);
            }
        }
        foreach (var topicId in updateRemoveExcludeTopic)
        {
            //getting in for each might not be best practice.but ...
            var updateTopic = await _useTopicRepository
                .GetQueryableSet()
                .FirstOrDefaultAsync(x => x.UserId == _currentUser.UserId && x.TopicId == topicId, cancellationToken);
            updateTopic.IsExcluded = false;

            if (updateTopic != null)
            {
                await _topicOfInterestService.UpdateAsync(updateTopic, cancellationToken);
            }
        }
        return new ApiResponse(statusCode: 200, "Success");
    }
}