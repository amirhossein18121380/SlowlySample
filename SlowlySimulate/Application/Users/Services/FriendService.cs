using Microsoft.EntityFrameworkCore;
using SlowlySimulate.Application.Common;
using SlowlySimulate.Application.Common.Services;
using SlowlySimulate.Domain.Models;
using SlowlySimulate.Domain.Repositories;

namespace SlowlySimulate.Application.Users.Services;
public class FriendService : CrudService<Friend>, IFriendService
{
    public FriendService(IRepository<Friend, Guid> userRepository, Dispatcher dispatcher)
        : base(userRepository, dispatcher)
    {
    }
    public async Task<ICollection<Guid>> GetSideBarFriends(Guid myUserId)
    {
        var myFriends = _repository.GetQueryableSet()
            .Where(f => (f.RequestedById == myUserId || f.RequestedToId == myUserId))
                .Select(f => f.RequestedById == myUserId ? f.RequestedToId : f.RequestedById);

        var result = await _repository.ToListAsync(myFriends);
        return result;
    }

    public async Task<List<Friend>> GetSentFriendRequestsAsync(Guid myUserId)
    {
        var sentRequests = _repository.GetQueryableSet()
            .Include(f => f.RequestedToId)
            .Where(f => f.RequestedById == myUserId);

        return await _repository.ToListAsync(sentRequests);
    }

    public async Task<List<Friend>> GetReceivedFriendRequestsAsync(Guid myUserId)
    {
        var sentRequests = _repository.GetQueryableSet()
            .Include(f => f.RequestedById)
            .Where(f => f.RequestedToId == myUserId);

        return await _repository.ToListAsync(sentRequests);
    }

}

