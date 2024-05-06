using IdentityModel;
using Microsoft.AspNetCore.SignalR;

namespace SlowlySimulate.Api.Providers
{
    public class UserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(JwtClaimTypes.Name)?.Value;
        }
    }
}
