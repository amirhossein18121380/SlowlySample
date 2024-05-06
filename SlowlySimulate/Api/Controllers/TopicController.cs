using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlowlySimulate.Api.Models.Topic;
using SlowlySimulate.Api.ViewModels.Topic;
using SlowlySimulate.Application.Common;
using SlowlySimulate.Application.Topic.Commands;
using SlowlySimulate.Application.Topic.Queries;
using SlowlySimulate.CrossCuttingConcerns.Models;
using SlowlySimulate.Persistence.Permissions;

namespace SlowlySimulate.Api.Controllers;

public class TopicController : Controller
{
    private readonly Dispatcher _dispatcher;

    public TopicController(Dispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [Authorize(Permissions.TopicPermission.Read)]
    public async Task<IActionResult> Index()
    {
        var response = await _dispatcher.DispatchAsync(new GetTopicsQuery());
        return View(response.ToModel());
    }


    [HttpPost]
    [Authorize(Permissions.TopicPermission.Create)]
    public async Task<ActionResult<ApiResponse>> Create([FromBody] AddTopicViewModel topic)
    {
        var response = await _dispatcher.DispatchAsync(new AddUpdateTopicCommand() { TopicName = topic.TopicName });
        return Ok(new ApiResponse(response.StatusCode, response.Message, response.Result));
    }


    [HttpGet]
    [Authorize(Permissions.TopicPermission.Read)]
    public async Task<ApiResponse> GetById(Guid topicId)
    {
        var response = await _dispatcher.DispatchAsync(new GetTopicQuery() { Id = topicId });
        return new ApiResponse(response.StatusCode, response.Message, response.Result);
    }

    [HttpPost]
    [Authorize(Permissions.TopicPermission.Update)]
    public async Task<IActionResult> Edit([FromBody] UpdateTopicViewModel topic)
    {
        var response = await _dispatcher.DispatchAsync(new AddUpdateTopicCommand() { TopicName = topic.TopicName, TopicId = topic.TopicId });
        return Ok(new ApiResponse(response.StatusCode, response.Message, response.Result));
    }


    [HttpDelete, ActionName("DeleteTopic")]
    [Authorize(Permissions.TopicPermission.Delete)]
    public async Task<ActionResult<ApiResponse>> DeleteTopic([FromBody] DeleteViewModel request)
    {
        var response = await _dispatcher.DispatchAsync(new DeleteTopicCommand() { TopicId = request.TopicId });
        return Ok(new ApiResponse(response.StatusCode, response.Message, response.Result));
    }
}