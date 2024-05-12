using Application.Common.Queries;
using Application.Decorators.AuditLog;
using Application.Decorators.DatabaseRetry;
using Application.TopicOfInterest.Services;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.TopicOfInterest.Queries;
public class GetSubTopicsQuery : IQuery<List<SubTopic>>
{
    public Guid UserId { get; set; }
    public Guid TopicId { get; set; }
}

[AuditLog]
[DatabaseRetry]
internal class GetSubTopicsQueryHandler : IQueryHandler<GetSubTopicsQuery, List<SubTopic>>
{
    private readonly IRepository<Domain.Models.SubTopic, Guid> _subTopicRepository;
    private readonly IRepository<UserTopic, Guid> _useTopicRepository;
    private readonly ISubTopicService _subTopicService;
    private readonly ILogger<GetSubTopicsQueryHandler> _logger;
    public GetSubTopicsQueryHandler(ISubTopicService subTopicService,
        IRepository<Domain.Models.SubTopic, Guid> subTopicRepository,
        IRepository<UserTopic, Guid> uTRepository,
        ILogger<GetSubTopicsQueryHandler> logger)
    {
        _subTopicService = subTopicService;
        _subTopicRepository = subTopicRepository;
        _useTopicRepository = uTRepository;
        _logger = logger;
    }

    public async Task<List<SubTopic>> HandleAsync(GetSubTopicsQuery query, CancellationToken cancellationToken = default)
    {
        var topicIds = await _useTopicRepository.GetQueryableSet()
            .Where(x => x.UserId == query.UserId & x.TopicId == query.TopicId)
            .Select(x => x.TopicId)
            .ToListAsync(cancellationToken);

        var subTopics = await _subTopicRepository.GetQueryableSet()
            .Where(subtopic => topicIds.Contains(subtopic.TopicId))
            .ToListAsync(cancellationToken);

        _logger.LogInformation("SubTopics fetched successfully.");
        return subTopics;
    }
}