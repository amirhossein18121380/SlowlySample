using Application.Common.Commands;
using Application.TopicOfInterest.Services;
using CrossCuttingConcerns.Models;
using Domain.Identity;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.TopicOfInterest.Commands;

public class AddTopicOfInterestCommand : ICommand<ApiResponse>
{
    public Guid UserId { get; set; }
    public List<Guid> TopicIds { get; set; }
}

internal class AddTopicOfInterestCommandHandler : ICommandHandler<AddTopicOfInterestCommand, ApiResponse>
{
    private readonly ITopicOfInterestService _topicOfInterestService;
    private readonly IRepository<Domain.Models.Topic, Guid> _topicRepository;
    private readonly IRepository<UserTopic, Guid> _useTopicRepository;
    private readonly ICurrentUser _currentUser;
    public AddTopicOfInterestCommandHandler(ITopicOfInterestService topicOfInterestService, IRepository<Domain.Models.Topic, Guid> topicRepository, IRepository<UserTopic, Guid> uTRepository,
        ICurrentUser currentUser)
    {
        _topicOfInterestService = topicOfInterestService;
        _topicRepository = topicRepository;
        _useTopicRepository = uTRepository;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse> HandleAsync(AddTopicOfInterestCommand command, CancellationToken cancellationToken = default)
    {
        var userTopicsHad = await _useTopicRepository
            .GetQueryableSet()
            .Where(x => x.UserId == _currentUser.UserId)
            .ToListAsync(cancellationToken);

        var newTopics = command.TopicIds.Except(userTopicsHad.Select(x => x.TopicId));

        var updateAddTopicOfInterest = command.TopicIds.Except(userTopicsHad.Where(x => x.IsExcluded != true & x.IsInterest == true).Select(x => x.TopicId));
        var updateRemoveTopicOfInterest = userTopicsHad.Where(x => x.IsExcluded != true & x.IsInterest == true).Select(x => x.TopicId).Except(command.TopicIds);


        foreach (var topicId in newTopics)
        {
            var userTopic = new Domain.Models.UserTopic
            {
                UserId = command.UserId,
                TopicId = topicId,
                IsExcluded = false,
                IsInterest = true
            };
            await _topicOfInterestService.AddAsync(userTopic, cancellationToken);
        }
        foreach (var topicId in updateAddTopicOfInterest)
        {
            //getting in for each might not be best practice.but ...
            var updateTopic = await _useTopicRepository
                .GetQueryableSet()
                .FirstOrDefaultAsync(x => x.UserId == _currentUser.UserId && x.TopicId == topicId, cancellationToken);
            updateTopic.IsInterest = true;
            updateTopic.IsExcluded = false;

            await _topicOfInterestService.UpdateAsync(updateTopic, cancellationToken);

        }

        foreach (var topicId in updateRemoveTopicOfInterest)
        {
            //getting in for each might not be best practice.but ...
            var updateTopic = await _useTopicRepository
                .GetQueryableSet()
                .FirstOrDefaultAsync(x => x.UserId == _currentUser.UserId && x.TopicId == topicId, cancellationToken);
            updateTopic.IsInterest = false;

            await _topicOfInterestService.UpdateAsync(updateTopic, cancellationToken);

        }
        return new ApiResponse(statusCode: 200, "Success");
    }
}