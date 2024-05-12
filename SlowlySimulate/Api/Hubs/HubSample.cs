using Application.Users.Services;
using Domain.Identity;
using Microsoft.AspNetCore.SignalR;

namespace SlowlySimulate.Api.Hubs;

public class HubSample : Hub<IHubSample>
{
    private readonly IUserService _userService;
    private readonly ICurrentUser _currentUser;
    public HubSample(ICurrentUser userIdProvider,
        IUserService userService)
    {
        _userService = userService;
        _currentUser = userIdProvider;
    }
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.Identity?.Name;

        if (userId != null)
        {
            await Clients.Client(Context.ConnectionId).Send($"Welcome to the chat, {userId}!");
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    public Task Send(string name, string message)
    {
        return Clients.All.Send($"{name}: {message}");
    }

    public Task SendToOthers(string name, string message)
    {
        return Clients.Others.Send($"{name}: {message}");
    }

    public Task SendToGroup(string groupName, string name, string message)
    {
        return Clients.Group(groupName).Send($"{name}@{groupName}: {message}");
    }

    public Task SendToOthersInGroup(string groupName, string name, string message)
    {
        return Clients.OthersInGroup(groupName).Send($"{name}@{groupName}: {message}");
    }

    public async Task JoinGroup(string groupName, string name)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        await Clients.Group(groupName).Send($"{name} joined {groupName}");
    }

    public async Task JoinCountryGroup()
    {
        var user = await _userService.GetByUserId(_currentUser.UserId);
        var groupName = $"{user.Country}" + "-Country";

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.OthersInGroup(groupName).Send($"{user.DisplayName} from your country, joined {groupName}");
    }

    public async Task LeaveGroup(string groupName, string name)
    {
        await Clients.Group(groupName).Send($"{name} left {groupName}");

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    public Task Echo(string name, string message)
    {
        return Clients.Caller.Send($"{name}: {message}");
    }
}

public interface IHubSample
{
    Task Send(string message);
}