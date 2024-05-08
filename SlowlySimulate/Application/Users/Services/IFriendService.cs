using SlowlySimulate.Application.Common.Services;
using SlowlySimulate.Domain.Models;

namespace SlowlySimulate.Application.Users.Services;

public interface IFriendService : ICrudService<Friend>
{
    Task<ICollection<Guid>> GetSideBarFriends(Guid myUserId);
    Task<List<Friend>> GetSentFriendRequestsAsync(Guid myUserId);
    Task<List<Friend>> GetReceivedFriendRequestsAsync(Guid myUserId);
}
