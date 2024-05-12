using Application.Common.Queries;
using Application.Decorators.AuditLog;
using Application.Decorators.DatabaseRetry;
using CrossCuttingConcerns.Models;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Topic.Queries;

public class GetTopicsQuery : IQuery<ApiResponse>
{
}

[AuditLog]
[DatabaseRetry]
internal class GetTopicsQueryHandler : IQueryHandler<GetTopicsQuery, ApiResponse>
{
    private readonly IRepository<Domain.Models.Topic, Guid> _topicRepository;
    private readonly ILogger<GetTopicsQueryHandler> _logger;
    public GetTopicsQueryHandler(IRepository<Domain.Models.Topic, Guid> topicRepository, ILogger<GetTopicsQueryHandler> logger)
    {
        _topicRepository = topicRepository;
        _logger = logger;
    }

    public async Task<ApiResponse> HandleAsync(GetTopicsQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _topicRepository.ToListAsync(_topicRepository.GetQueryableSet());
        if (result.Count > 0)
        {
            return new ApiResponse(200, "Fetched Successfully.", result);
        }
        return new ApiResponse(400, "Cannot Fetch data.");
    }
}
