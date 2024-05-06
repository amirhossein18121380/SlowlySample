using SlowlySimulate.Application.Common.Queries;
using SlowlySimulate.Application.Decorators.AuditLog;
using SlowlySimulate.Application.Decorators.DatabaseRetry;
using SlowlySimulate.Domain.Identity;
using SlowlySimulate.Domain.Models;
using SlowlySimulate.Domain.Repositories;

namespace SlowlySimulate.Application.TopicOfInterest.Queries;
public class GetUserExcludedTopicsQuery : IQuery<List<Domain.Models.Topic>>
{
}

[AuditLog]
[DatabaseRetry]
internal class GetUserExcludedTopicsQueryHandler : IQueryHandler<GetUserExcludedTopicsQuery, List<Domain.Models.Topic>>
{
    private readonly IRepository<Domain.Models.Topic, Guid> _topicRepository;
    private readonly IRepository<UserTopic, Guid> _useTopicRepository;
    private readonly ICurrentUser _currentUser;
    public GetUserExcludedTopicsQueryHandler(IRepository<Domain.Models.Topic, Guid> topicRepository, IRepository<UserTopic, Guid> uTRepository,
        ICurrentUser currentUser)
    {
        _topicRepository = topicRepository;
        _useTopicRepository = uTRepository;
        _currentUser = currentUser;
    }

    public async Task<List<Domain.Models.Topic>> HandleAsync(GetUserExcludedTopicsQuery query, CancellationToken cancellationToken = default)
    {
        var getQuery = _useTopicRepository.GetQueryableSet().Where(x => x.UserId == _currentUser.UserId && x.IsExcluded == true)
            .Select(x => x.TopicId);

        var excludedTopicIds = await _useTopicRepository.ToListAsync(getQuery);

        var topics = _topicRepository.GetQueryableSet()
            .Where(topic => excludedTopicIds.Contains(topic.Id))
            .ToList();

        return topics;
    }
}
