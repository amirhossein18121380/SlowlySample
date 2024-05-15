using Application.Common;
using Application.Users.Queries;
using Application.Users.Services;
using AutoMapper;
using CrossCuttingConcerns.DateTimes;
using Domain.Identity;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace SlowlySimulate.Api.Controllers;


public class AppHomeController : Controller
{
    private readonly Dispatcher _dispatcher;
    private readonly IUserService _userService;
    private readonly IFriendShipService _friendService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUser _currentUser;
    private Guid UserId = Guid.Empty;
    private readonly IMapper _mapper;


    public AppHomeController(Dispatcher dispatcher,
        IDateTimeProvider dateTimeProvider,
        ICurrentUser currentUser,
        IMapper mapper,
        IUserService userService,
        IFriendShipService friendService)
    {
        _dispatcher = dispatcher;
        _dateTimeProvider = dateTimeProvider;
        _currentUser = currentUser;
        _mapper = mapper;
        _userService = userService;
        UserId = _currentUser.IsAuthenticated ? _currentUser.UserId : Guid.Empty;
        _friendService = friendService;
    }

    public async Task<IActionResult> Index()
    {
        var friends = await GetFriendsWithCount();
        return View(friends);
    }


    public async Task<(List<SlowlyUser> Models, int Count)> GetFriendsWithCount()
    {
        var profileModel = await _dispatcher.DispatchAsync(new GetUserFriendsQuery());
        if (profileModel.Result is List<SlowlyUser> models)
        {
            return (models, models.Count);
        }

        return (new List<SlowlyUser>(), 0);
    }

    [HttpGet]
    public async Task<List<SlowlyUser>> GetNonFriends()
    {
        var nonF = await _userService.GetNonFriends(_currentUser.UserId);
        if (nonF.Any())
        {
            return (nonF);
        }

        return new List<SlowlyUser>();
    }


    [HttpPost]
    public async Task<IActionResult> AddFriends([FromBody] List<string> ids)
    {
        // Convert list of strings to list of GUIDs
        List<Guid> guids = ids.Select(id => Guid.Parse(id)).ToList();

        // Call your friend service method passing the list of GUIDs
        await _friendService.AddFriends(_currentUser.UserId, guids);

        // Return an appropriate response
        return Ok(); // Or any other IActionResult you want
    }

}