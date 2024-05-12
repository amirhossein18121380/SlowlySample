

using Domain.AuthorizationDefinitions;
using Humanizer;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace Persistence
{
    public class IdentityServerConfig
    {
        public const string LocalApiName = "LocalAPI";
        public const string SwaggerClientID = "swaggerui";

        // https://identityserver4.readthedocs.io/en/latest/reference/identity_resource.html
        public static readonly IEnumerable<IdentityResource> GetIdentityResources =
            new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Phone(),
                new IdentityResources.Email(),
                new IdentityResource(ScopeConstants.Roles, new List<string> { JwtClaimTypes.Role })
            };

        // https://identityserver4.readthedocs.io/en/latest/reference/api_scope.html
        public static readonly IEnumerable<ApiScope> GetApiScopes =
            new[]
            {
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName),

                new ApiScope(LocalApiName, "My API")
            };

        // https://identityserver4.readthedocs.io/en/latest/reference/api_resource.html
        public static readonly IEnumerable<ApiResource> GetApiResources =
            new[]
            {
                new ApiResource(LocalApiName) {
                    DisplayName = LocalApiName.Humanize(LetterCasing.Title),
                    Scopes = { IdentityServerConstants.LocalApi.ScopeName, LocalApiName },
                    UserClaims = {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Email,
                        JwtClaimTypes.PhoneNumber,
                        JwtClaimTypes.Role,
                        ApplicationClaimTypes.Permission,
                        Policies.IsUser,
                        Policies.IsAdmin
                    }
                }
            };

        // https://identityserver4.readthedocs.io/en/latest/reference/client.html
        public static readonly IEnumerable<Client> GetClients =
            new[]
            {
                new Client
                {
                    ClientId = SwaggerClientID,
                    ClientName = "Swagger UI",
                    //AllowedGrantTypes = OidcConstants.GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,
                    RequireClientSecret = false,
                    RequirePkce = true,

                    RedirectUris = {
                        "http://127.0.0.1:64879",
                        "http://localhost:53414/swagger/oauth2-redirect.html",
                        "https://blazor-server.quarella.net/swagger/oauth2-redirect.html" },

                    AllowedScopes = {
                        OidcConstants.StandardScopes.OpenId,
                        OidcConstants.StandardScopes.Profile,
                        OidcConstants.StandardScopes.Phone,
                        OidcConstants.StandardScopes.Email,
                        ScopeConstants.Roles,
                        IdentityServerConstants.LocalApi.ScopeName
                    }
                },

                new Client
                {
                    ClientClaimsPrefix = string.Empty,

                    ClientId = "clientToDo",

                    // no interactive user, use the clientid/secret for authentication
                    //AllowedGrantTypes = OidcConstants.GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { LocalApiName },

                    Claims =
                    {
                        new ClientClaim(ApplicationClaimTypes.Permission, "Todo.Delete")
                    }
                }
            };

    }
}
