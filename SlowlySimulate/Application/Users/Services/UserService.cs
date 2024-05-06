using Microsoft.EntityFrameworkCore;
using SlowlySimulate.Application.Common;
using SlowlySimulate.Application.Common.Services;
using SlowlySimulate.Domain.Models;
using SlowlySimulate.Domain.Repositories;

namespace SlowlySimulate.Application.Users.Services;

public class UserService : CrudService<SlowlyUser>, IUserService
{
    private readonly IFriendService _friendService;
    private readonly IRepository<Friend, Guid> _friendRepository;
    public UserService(IRepository<SlowlyUser, Guid> userRepository, Dispatcher dispatcher,
        IFriendService friendService, IRepository<Friend, Guid> friendRepository)
        : base(userRepository, dispatcher)
    {
        _friendService = friendService;
        _friendRepository = friendRepository;
    }

    public async Task<List<SlowlyUser>> GetSameCountryUsers(Guid userId)
    {
        var query = _repository.GetQueryableSet();
        var user = await _repository.FirstOrDefaultAsync(query.Where(x => x.UserId == userId));

        var list = query.Where(x => x.Country == user.Country).ToList();
        return list;
    }

    public async Task<string?> GetCountry(Guid userId)
    {
        var query = _repository.GetQueryableSet().Where(x => x.UserId == userId);
        var user = await _repository.FirstOrDefaultAsync(query);

        return user?.Country;
    }

    public async Task<SlowlyUser> GetByUserId(Guid userId)
    {
        var query = _repository.GetQueryableSet().Where(x => x.UserId == userId);
        var user = await _repository.SingleOrDefaultAsync(query);
        return user;
    }

    public async Task<List<SlowlyUser>> GetFriends(Guid userId)
    {
        var users = await _repository.ToListAsync(
            _repository.GetQueryableSet()
                .Include(u => u.Friends.Where(f => f.RequestedById == userId))
                .Include(u => u.Friends.Where(f => f.RequestedToId == userId))
        );
        return users;
    }

    public async Task<List<SlowlyUser>> GetFriendsStartedByMe(Guid userId)
    {
        var users = await _repository.ToListAsync(
            _repository.GetQueryableSet()
                .Include(u => u.Friends.Where(f => f.RequestedById == userId))
        );
        return users;
    }
    public async Task<List<SlowlyUser>> GetFriendsStartedByOthers(Guid userId)
    {
        var users = await _repository.ToListAsync(
            _repository.GetQueryableSet()
                .Include(u => u.Friends.Where(f => f.RequestedToId == userId))
        );
        return users;
    }

    public async Task<List<SlowlyUser>> GetSideBarFriends(Guid userId)
    {
        var users = await _repository.ToListAsync(
            _repository.GetQueryableSet()
                .Include(u => u.Friends.Where(f =>
                    (f.RequestedById == userId || f.RequestedToId == userId) &&
                    !f.IsHiddenUser && !f.IsRemovedUser && !f.IsReportedUser
                ))
        );

        return users;
    }

}