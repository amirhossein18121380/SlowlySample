using SlowlySimulate.Application.Common.Queries;
using SlowlySimulate.Application.Decorators.AuditLog;
using SlowlySimulate.Application.Decorators.DatabaseRetry;
using SlowlySimulate.CrossCuttingConcerns.Models;
using SlowlySimulate.Domain.Repositories;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace SlowlySimulate.Application.Topic.Queries;

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
            return new ApiResponse(Status200OK, "Fetched Successfully.", result);
        }
        return new ApiResponse(Status400BadRequest, "Cannot Fetch data.");
    }
}
