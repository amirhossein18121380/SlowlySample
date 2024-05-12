using Application.Common.Queries;
using CrossCuttingConcerns.Exceptions;
using CrossCuttingConcerns.Models;
using Domain.Repositories;

namespace Application.Topic.Queries;

public class GetTopicQuery : IQuery<ApiResponse>
{
    public Guid Id { get; set; }
    public bool ThrowNotFoundIfNull { get; set; }
}

internal class GetTopicQueryHandler : IQueryHandler<GetTopicQuery, ApiResponse>
{
    private readonly IRepository<Domain.Models.Topic, Guid> _topicRepository;

    public GetTopicQueryHandler(IRepository<Domain.Models.Topic, Guid> topicRepository)
    {
        _topicRepository = topicRepository;
    }

    public async Task<ApiResponse> HandleAsync(GetTopicQuery query, CancellationToken cancellationToken = default)
    {
        var topic = await _topicRepository.FirstOrDefaultAsync(_topicRepository.GetQueryableSet().Where(x => x.Id == query.Id));

        if (query.ThrowNotFoundIfNull && topic == null)
        {
            return new ApiResponse(statusCode: 404, new NotFoundException($"Topic {query.Id} not found.").Message);
        }

        return new ApiResponse(statusCode: 200, "Success", topic);
    }
}
