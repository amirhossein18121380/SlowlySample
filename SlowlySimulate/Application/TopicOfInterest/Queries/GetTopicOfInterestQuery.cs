using SlowlySimulate.Application.Common.Queries;
using SlowlySimulate.Application.Decorators.AuditLog;
using SlowlySimulate.Application.Decorators.DatabaseRetry;
using SlowlySimulate.Domain.Identity;
using SlowlySimulate.Domain.Models;
using SlowlySimulate.Domain.Repositories;

namespace SlowlySimulate.Application.TopicOfInterest.Queries;

public class GetTopicOfInterestQuery : IQuery<List<Domain.Models.Topic>>
{
}

[AuditLog]
[DatabaseRetry]
internal class GetTopicsQueryHandler : IQueryHandler<GetTopicOfInterestQuery, List<Domain.Models.Topic>>
{
    private readonly IRepository<Domain.Models.Topic, Guid> _topicRepository;
    private readonly IRepository<UserTopic, Guid> _useTopicRepository;
    private readonly ICurrentUser _currentUser;
    public GetTopicsQueryHandler(IRepository<Domain.Models.Topic, Guid> topicRepository, IRepository<UserTopic, Guid> uTRepository,
        ICurrentUser currentUser)
    {
        _topicRepository = topicRepository;
        _useTopicRepository = uTRepository;
        _currentUser = currentUser;
    }

    public async Task<List<Domain.Models.Topic>> HandleAsync(GetTopicOfInterestQuery query, CancellationToken cancellationToken = default)
    {
        var getQuery = _useTopicRepository.GetQueryableSet().Where(x => x.UserId == _currentUser.UserId && x.IsInterest == true)
            .Select(x => x.TopicId);

        var topicIds = await _useTopicRepository.ToListAsync(getQuery);

        var topics = _topicRepository.GetQueryableSet()
            .Where(topic => topicIds.Contains(topic.Id))
            .ToList();

        return topics;
    }
}
