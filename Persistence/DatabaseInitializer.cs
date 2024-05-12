using CrossCuttingConcerns.Exceptions;
using CrossCuttingConcerns.Storage;
using Domain.AuthorizationDefinitions;
using Domain.Constants;
using Domain.Models;
using Domain.Permissions;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Persistence
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly SlowlyDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly EntityPermissions _entityPermissions;
        private readonly ILogger _logger;
        public DatabaseInitializer(
            SlowlyDbContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            EntityPermissions entityPermissions,
            ILogger<DatabaseInitializer> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _entityPermissions = entityPermissions;
            _logger = logger;
        }

        public virtual async Task SeedAsync()
        {
            await MigrateAsync();

            await EnsureAdminIdentitiesAsync();

            _context.Database.ExecuteSqlRaw("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
        }

        private async Task MigrateAsync()
        {
            await _context.Database.MigrateAsync();
        }

        public async Task EnsureAdminIdentitiesAsync()
        {
            await EnsureRole(DefaultRoleNames.Administrator, _entityPermissions.GetAllPermissionValues());

            foreach (var userFeature in Enum.GetValues<UserFeatures>())
                await EnsureRole(userFeature.ToString(), _entityPermissions.GetAllPermissionValuesFor(userFeature));

            await CreateUser(DefaultUserNames.Administrator, "Admin123;", "admin@slowly.com", "+1 (123) 456-7890", new string[] { DefaultRoleNames.Administrator });

            _logger.LogInformation("Inbuilt account generation completed");
        }

        private async Task EnsureRole(string roleName, IEnumerable<string> claims)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                if (claims == null)
                    claims = Array.Empty<string>();

                string[] invalidClaims = claims.Where(c => _entityPermissions.GetPermissionByValue(c) == null).ToArray();
                if (invalidClaims.Any())
                    throw new Exception("The following claim types are invalid: " + string.Join(", ", invalidClaims));

                Role applicationRole = new(roleName);

                var result = await _roleManager.CreateAsync(applicationRole);

                role = await _roleManager.FindByNameAsync(applicationRole.Name);

                foreach (string claim in claims.Distinct())
                {
                    result = await _roleManager.AddClaimAsync(role, new Claim(ApplicationClaimTypes.Permission, _entityPermissions.GetPermissionByValue(claim)));

                    if (!result.Succeeded)
                    {
                        await _roleManager.DeleteAsync(role);

                        throw new DomainException($"Unable to add claim {claim} to role {roleName}");
                    }
                }
            }

            var roleClaims = (await _roleManager.GetClaimsAsync(role)).Select(c => c.Value).ToList();
            var newClaims = claims.Except(roleClaims);

            foreach (string claim in newClaims)
                await _roleManager.AddClaimAsync(role, new Claim(ApplicationClaimTypes.Permission, claim));

            var deprecatedClaims = roleClaims.Except(claims);
            var roles = await _roleManager.Roles.ToListAsync();

            foreach (string claim in deprecatedClaims)
                foreach (var r in roles)
                    await _roleManager.RemoveClaimAsync(r, new Claim(ApplicationClaimTypes.Permission, claim));
        }

        public async Task<SlowlyUser> SaveNewUserAsync(Guid userId)
        {
            var cancellationToken = new CancellationToken();
            var slowlyUser = new SlowlyUser()
            {
                UserId = userId,
                CreatedAt = DateTime.Now
            };
            slowlyUser.SetRandomSlowlyId();

            await _context.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken);
            var response = await _context.SlowlyUsers.AddAsync(slowlyUser, cancellationToken);
            await _context.CommitTransactionAsync(cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return response.Entity;
        }


        private async Task<User> CreateUser(string userName, string password, string email, string phoneNumber, string[] roles = null)
        {
            var applicationUser = _userManager.FindByNameAsync(userName).Result;

            if (applicationUser == null)
            {
                applicationUser = new User
                {
                    UserName = userName,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    EmailConfirmed = true
                };

                var result = _userManager.CreateAsync(applicationUser, password).Result;

                if (!result.Succeeded)
                    throw new Exception(result.Errors.First().Description);

                result = _userManager.AddClaimsAsync(applicationUser, new Claim[]{
                        new Claim(JwtClaimTypes.Name, userName),
                        new Claim(JwtClaimTypes.Email, email),
                        new Claim(JwtClaimTypes.EmailVerified, ClaimValues.trueString, ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.PhoneNumber, phoneNumber)
                    }).Result;

                if (!result.Succeeded)
                    throw new Exception(result.Errors.First().Description);

                //add claims version of roles
                if (roles != null)
                {
                    foreach (var role in roles.Distinct())
                    {
                        await _userManager.AddClaimAsync(applicationUser, new Claim($"Is{role}", ClaimValues.trueString));
                    }

                    User user = await _userManager.FindByNameAsync(applicationUser.UserName);

                    try
                    {
                        result = await _userManager.AddToRolesAsync(user, roles.Distinct());
                    }
                    catch
                    {
                        await _userManager.DeleteAsync(user);
                        throw;
                    }

                    if (!result.Succeeded)
                    {
                        await _userManager.DeleteAsync(user);
                    }
                }

                await SaveNewUserAsync(applicationUser.Id);
            }

            return applicationUser;
        }
    }
}