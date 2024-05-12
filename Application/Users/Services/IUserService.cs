using Application.Common.Services;
using Domain.Models;

namespace Application.Users.Services;

public interface IUserService : ICrudService<SlowlyUser>
{
    Task<List<SlowlyUser>> GetSameCountryUsers(Guid userId);
    Task<string?> GetCountry(Guid userId);
    Task<SlowlyUser> GetByUserId(Guid userId);
    Task<List<SlowlyUser>> GetSideBarFriends(Guid userId);
    Task<List<SlowlyUser>> GetNonFriends(Guid userId);
}
