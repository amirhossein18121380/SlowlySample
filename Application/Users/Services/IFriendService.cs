using Application.Common.Services;
using Domain.Models;

namespace Application.Users.Services;

public interface IFriendService : ICrudService<Friend>
{
    Task<ICollection<Guid>> GetSideBarFriends(Guid myUserId);
    Task<List<Friend>> GetSentFriendRequestsAsync(Guid myUserId);
    Task<List<Friend>> GetReceivedFriendRequestsAsync(Guid myUserId);
    Task<ICollection<Guid>> GetAllFriendsIncludeMe(Guid myUserId);
    Task<ICollection<Guid>> GetAllMyFriends(Guid myUserId);
    Task AddFriends(Guid currentUser, List<Guid> ids);
}
