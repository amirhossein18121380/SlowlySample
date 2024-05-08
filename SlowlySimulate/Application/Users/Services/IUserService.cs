using SlowlySimulate.Application.Common.Services;
using SlowlySimulate.Domain.Models;

namespace SlowlySimulate.Application.Users.Services;

public interface IUserService : ICrudService<SlowlyUser>
{
    Task<List<SlowlyUser>> GetSameCountryUsers(Guid userId);
    Task<string?> GetCountry(Guid userId);
    Task<SlowlyUser> GetByUserId(Guid userId);
    Task<List<SlowlyUser>> GetSideBarFriends(Guid userId);
}
