using Application.Common;
using Application.Topic.Queries;
using Grpc.Core;
using SlowlySimulate.Protos;

namespace SlowlySimulate.Api.Controllers;


public class GetTopicByIdGrpcEndpoint : GetTopicByIdGrpcService.GetTopicByIdGrpcServiceBase
{
    private readonly ILogger<GetTopicByIdGrpcEndpoint> _logger;
    private readonly Dispatcher _dispatcher;
    public GetTopicByIdGrpcEndpoint(Dispatcher dispatcher, ILogger<GetTopicByIdGrpcEndpoint> logger)
    {
        _dispatcher = dispatcher;
        _logger = logger;
    }

    public override async Task<GrpcApiResponse> Handle(GetTopicQueryByIdGrpcRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"Received request for ID: {request.Id}");
        var response = await _dispatcher.DispatchAsync(new GetTopicQuery() { Id = new Guid(request.Id) });
        var res = new GrpcApiResponse
        {
            StatusCode = response.StatusCode,
            Message = response.Message
        };

        return res;
    }
}