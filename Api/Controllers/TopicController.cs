
using Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("[controller]")]
public class TopicController : ControllerBase
{
    private readonly Dispatcher _dispatcher;
    public TopicController(Dispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    //[HttpPost]
    //[Authorize(Permissions.TopicPermission.Create)]
    //public async Task<ApiResponse> Create([FromBody] AddTopicViewModel topic)
    //{
    //    var response = await _dispatcher.DispatchAsync(new AddUpdateTopicCommand() { TopicName = topic.TopicName });
    //    return new ApiResponse(response.StatusCode, response.Message, response.Result);
    //}
}

