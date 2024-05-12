using Domain.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace Domain.AuthorizationDefinitions
{
    public class AuthorizeForFeature : AuthorizeAttribute
    {
        public AuthorizeForFeature(UserFeatures userFeature) : base($"Is{userFeature}")
        {

        }
    }
}
