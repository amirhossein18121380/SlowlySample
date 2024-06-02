using Application.Common;
using Application.Topic.Commands;
using Application.Topic.Queries;
using CrossCuttingConcerns.Models;
using Domain.Permissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlowlySimulate.Api.Models.Topic;
using SlowlySimulate.Api.ViewModels.Topic;

namespace SlowlySimulate.Api.Controllers;

public class TopicController : Controller
{
    private readonly Dispatcher _dispatcher;
    private readonly IMediator _mediator;

    public TopicController(Dispatcher dispatcher, IMediator mediator)
    {
        _dispatcher = dispatcher;
        _mediator = mediator;
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