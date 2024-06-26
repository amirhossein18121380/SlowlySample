﻿using Application.Common;
using Application.Common.Services;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Services;

public class UserService : CrudService<SlowlyUser>, IUserService
{
    private readonly IFriendShipService _friendService;
    private readonly IRepository<FriendShip, Guid> _friendRepository;
    public UserService(IRepository<SlowlyUser, Guid> userRepository, Dispatcher dispatcher,
        IFriendShipService friendService, IRepository<FriendShip, Guid> friendRepository)
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

    public async Task<List<SlowlyUser>> GetNonFriends(Guid userId)
    {
        var users = new List<SlowlyUser>();
        var friends = await _friendService.GetAllFriendsIncludeMe(userId);
        var nonFriends = _repository.GetQueryableSet().Select(x => x.UserId).Except(friends).AsQueryable();
        foreach (var f in nonFriends)
        {
            var query = await GetByUserId(f);

            if (query != null)
            {
                users.Add(query);
            }
        }

        return users;
    }


    public async Task<List<SlowlyUser>> GetSideBarFriends(Guid userId)
    {
        var users = new List<SlowlyUser>();
        var friends = await _friendService.GetSideBarFriends(userId);
        var user = await _repository.GetQueryableSet().SingleOrDefaultAsync(x => x.UserId == userId);

        foreach (var f in friends)
        {
            var query = await GetByUserId(f);

            if (query != null)
            {
                users.Add(query);
            }
        }

        return users;
    }

}