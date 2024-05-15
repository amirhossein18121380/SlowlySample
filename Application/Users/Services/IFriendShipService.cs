using Application.Common.Services;
using Domain.Models;

namespace Application.Users.Services;

public interface IFriendShipService : ICrudService<FriendShip>
{
    Task<ICollection<Guid>> GetSideBarFriends(Guid myUserId);
    Task<List<FriendShip>> GetSentFriendRequestsAsync(Guid myUserId);
    Task<List<FriendShip>> GetReceivedFriendRequestsAsync(Guid myUserId);
    Task<ICollection<Guid>> GetAllFriendsIncludeMe(Guid myUserId);
    Task<ICollection<Guid>> GetAllMyFriends(Guid myUserId);
    Task AddFriends(Guid currentUser, List<Guid> ids);
}
