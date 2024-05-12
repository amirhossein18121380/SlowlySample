using Application.Common;
using Application.Users.Commands;
using Application.Users.Queries;
using CrossCuttingConcerns.DateTimes;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SlowlySimulate.Api.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly Dispatcher _dispatcher;
    private readonly UserManager<User> _userManager;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UsersController(Dispatcher dispatcher,
        UserManager<User> userManager,
        ILogger<UsersController> logger,
        IDateTimeProvider dateTimeProvider)
    {
        _dispatcher = dispatcher;
        _userManager = userManager;
        _dateTimeProvider = dateTimeProvider;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SlowlyUser>> Get(Guid id)
    {
        var user = await _dispatcher.DispatchAsync(new GetUserQuery { Id = id });
        return Ok(user);
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var user = await _dispatcher.DispatchAsync(new GetUserQuery { Id = id });
        await _dispatcher.DispatchAsync(new DeleteUserCommand { User = user });

        return Ok();
    }
}