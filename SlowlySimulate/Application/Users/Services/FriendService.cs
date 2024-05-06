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

}

