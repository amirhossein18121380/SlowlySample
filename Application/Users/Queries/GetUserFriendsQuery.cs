using Application.Common.Queries;
using Application.Users.Services;
using CrossCuttingConcerns.Models;
using Domain.Identity;

namespace Application.Users.Queries;

public class GetUserFriendsQuery : IQuery<ApiResponse>
{
}

internal class GetUserFriendsQueryHandler : IQueryHandler<GetUserFriendsQuery, ApiResponse>
{
    private readonly IUserService _userService;
    private readonly ICurrentUser _currentUser;

    public GetUserFriendsQueryHandler(IUserService userService, ICurrentUser currentUser)
    {
        _userService = userService;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse> HandleAsync(GetUserFriendsQuery query, CancellationToken cancellationToken = default)
    {
        var userFriends = await _userService.GetSideBarFriends(_currentUser.UserId);
        if (userFriends.Count == 0)
        {
            return new ApiResponse(statusCode: 404, "Not Any friends");
        }

        return new ApiResponse(statusCode: 200, "Fetched Successfully", userFriends);
    }
}
