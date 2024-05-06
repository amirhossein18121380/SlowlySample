using Microsoft.AspNetCore.Authorization;

namespace SlowlySimulate.Infrastructure.AuthorizationDefinitions
{
    public class AuthorizeForFeature : AuthorizeAttribute
    {
        public AuthorizeForFeature(UserFeatures userFeature) : base($"Is{userFeature}")
        {

        }
    }
}
