using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SlowlySimulate.Application.Common;
using SlowlySimulate.Application.Users.Commands;
using SlowlySimulate.Application.Users.Queries;
using SlowlySimulate.CrossCuttingConcerns.DateTimes;
using SlowlySimulate.Domain.Models;

namespace SlowlySimulate.Api.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly Dispatcher _dispatcher;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UsersController(Dispatcher dispatcher,
        UserManager<ApplicationUser> userManager,
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