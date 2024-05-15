using Application.Common;
using Application.Common.Services;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Services;
public class FriendShipService : CrudService<FriendShip>, IFriendShipService
{
    public FriendShipService(IRepository<FriendShip, Guid> userRepository, Dispatcher dispatcher)
        : base(userRepository, dispatcher)
    {
    }

    public async Task<ICollection<Guid>> GetAllMyFriends(Guid myUserId)
    {
        var myFriendsId = _repository.GetQueryableSet()
            .Where(f => (f.RequestedById == myUserId || f.RequestedToId == myUserId))
            .Select(f => f.RequestedById == myUserId ? f.RequestedToId : f.RequestedById);

        var friends = await _repository.ToListAsync(myFriendsId);
        return friends;
    }

    public async Task<ICollection<Guid>> GetAllFriendsIncludeMe(Guid myUserId)
    {
        // Combine conditions in a single Where clause for efficiency
        var friendsQuery = _repository.GetQueryableSet()
            .Where(f => f.RequestedById == myUserId || f.RequestedToId == myUserId);
        var friendUserIds = await friendsQuery.Select(f => f.RequestedById == myUserId ? f.RequestedToId : (Guid?)f.RequestedById)
            .ToListAsync();

        friendUserIds.Add(myUserId);

        return friendUserIds.Where(id => id.HasValue).Select(id => id.Value).ToList();
    }

    public async Task<ICollection<Guid>> GetSideBarFriends(Guid myUserId)
    {
        var myFriends = _repository.GetQueryableSet()
            .Where(f => (f.RequestedById == myUserId || f.RequestedToId == myUserId)
                        && !f.IsHiddenUser && !f.IsRemovedUser && !f.IsReportedUser)
            .Select(f => f.RequestedById == myUserId ? f.RequestedToId : f.RequestedById);

        var result = await _repository.ToListAsync(myFriends);
        return result;
    }
    public async Task<List<FriendShip>> GetSentFriendRequestsAsync(Guid myUserId)
    {
        var sentRequests = _repository.GetQueryableSet()
            .Include(f => f.RequestedToId)
            .Where(f => f.RequestedById == myUserId);

        return await _repository.ToListAsync(sentRequests);
    }
    public async Task<List<FriendShip>> GetReceivedFriendRequestsAsync(Guid myUserId)
    {
        var sentRequests = _repository.GetQueryableSet()
            .Include(f => f.RequestedById)
            .Where(f => f.RequestedToId == myUserId);

        return await _repository.ToListAsync(sentRequests);
    }
    public Task AddFriends(Guid currentUser, List<Guid> ids)
    {
        var friends = new List<FriendShip>();
        foreach (var id in ids)
        {
            var friend = new FriendShip()
            {
                RequestedById = currentUser,
                RequestedToId = id,
            };
            friends.Add(friend);
        }

        if (friends.Count > 0)
        {
            _repository.BulkInsert(friends);
        }

        return Task.CompletedTask;
    }
}

