﻿using CrossCuttingConcerns.DateTimes;
using CrossCuttingConcerns.Models;
using Domain.AuthorizationDefinitions;
using Domain.Constants;
using Domain.Dto.Profile;
using Domain.Dto.Role;
using Domain.Extension;
using Domain.Models;
using Domain.Permissions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;


namespace Application.Roles.Service;

public class RoleService : IRoleService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<RoleService> _logger;
    private readonly EntityPermissions _entityPermissions;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RoleService(UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ILogger<RoleService> logger,
        EntityPermissions entityPermissions,
        IDateTimeProvider dateTimeProvider
        )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _entityPermissions = entityPermissions;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ApiResponse> GetUsers(int pageSize = 10, int pageNumber = 0)
    {
        var userList = _userManager.Users.AsQueryable();
        var count = userList.Count();
        var listResponse = userList.OrderBy(x => x.Id).Skip(pageNumber * pageSize).Take(pageSize).ToList();

        var userDtoList = new List<UserDto>(); // This sucks, but Select isn't async happy, and the passing into a 'ProcessEventAsync' is another level of misdirection
        foreach (var applicationUser in listResponse)
        {
            userDtoList.Add(new UserDto
            {
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                UserName = applicationUser.UserName,
                Email = applicationUser.Email,
                UserId = applicationUser.Id,
                Roles = await _userManager.GetRolesAsync(applicationUser).ConfigureAwait(true) as List<string>
            });
        }

        return new ApiResponse(200, $"{count} users fetched", userDtoList);
    }
    public ApiResponse GetPermissions()
    {
        var permissions = _entityPermissions.GetAllPermissionNames();
        return new ApiResponse(200, "Permissions list fetched", permissions);
    }

    #region Roles

    public async Task<ApiResponse> GetRolesPermissionsAsync(string roleName)
    {
        var role = _roleManager.Roles.AsQueryable().SingleOrDefault(x => x.Name == roleName);
        var claims = await _roleManager.GetClaimsAsync(role);
        List<string> permissions = claims.OrderBy(x => x.Value).Where(x => x.Type == ApplicationClaimTypes.Permission).Select(x => _entityPermissions.GetPermissionByValue(x.Value).Name).ToList();
        return new ApiResponse(200, $"Role already exists", permissions); ;
    }
    public async Task<ApiResponse> GetRolePermissionAsync(string roleName)
    {
        var role = _roleManager.Roles.AsQueryable().SingleOrDefault(x => x.Name == roleName);

        var claims = await _roleManager.GetClaimsAsync(role);
        List<string> permissions = claims.OrderBy(x => x.Value).Where(x => x.Type == ApplicationClaimTypes.Permission)
            .Select(x => _entityPermissions.GetPermissionByValue(x.Value).Name).ToList();

        var roleDto = new RoleDto
        {
            Name = role.Name,
            Permissions = permissions
        };

        return new ApiResponse(200, $"Role already exists", roleDto);
    }
    public async Task<ApiResponse> GetRolesAsync(int pageSize = 0, int pageNumber = 0)
    {
        var roleQuery = _roleManager.Roles.AsQueryable().OrderBy(x => x.Name);
        var count = roleQuery.Count();
        var listResponse = (pageSize > 0 ? roleQuery.Skip(pageNumber * pageSize).Take(pageSize) : roleQuery).ToList();

        var roleDtoList = new List<RoleDto>();

        foreach (var role in listResponse)
        {
            var claims = await _roleManager.GetClaimsAsync(role);
            List<string> permissions = claims.OrderBy(x => x.Value).Where(x => x.Type == ApplicationClaimTypes.Permission).Select(x => _entityPermissions.GetPermissionByValue(x.Value).Name).ToList();

            roleDtoList.Add(new RoleDto
            {
                Name = role.Name,
                Permissions = permissions
            });
        }

        return new ApiResponse(200, $"Role {count} already exists", roleDtoList);
    }
    public async Task<ApiResponse> GetRoleAsync(string roleName)
    {
        var identityRole = await _roleManager.FindByNameAsync(roleName);

        var claims = await _roleManager.GetClaimsAsync(identityRole);
        var permissions = claims.OrderBy(x => x.Value).Where(x => x.Type == ApplicationClaimTypes.Permission).Select(x => _entityPermissions.GetPermissionByValue(x.Value).Name).ToList();

        var roleDto = new RoleDto
        {
            Name = roleName,
            Permissions = permissions
        };

        return new ApiResponse(200, "Role fetched", roleDto);
    }
    public async Task<ApiResponse> CreateRoleAsync(RoleDto roleDto)
    {
        if (_roleManager.Roles.Any(r => r.Name == roleDto.Name))
            return new ApiResponse(400, $"Role {roleDto.Name} already exists");
        var result = await _roleManager.CreateAsync(new Role(roleDto.Name));

        if (!result.Succeeded)
        {
            var msg = result.GetErrors();
            _logger.LogWarning($"Error while creating role {roleDto.Name}: {msg}");
            return new ApiResponse(400, msg);
        }

        // Re-create the permissions
        var role = await _roleManager.FindByNameAsync(roleDto.Name);

        foreach (var claim in roleDto.Permissions)
        {
            var resultAddClaim = await _roleManager.AddClaimAsync(role, new Claim(ApplicationClaimTypes.Permission, _entityPermissions.GetPermissionByName(claim)));

            if (!resultAddClaim.Succeeded)
                await _roleManager.DeleteAsync(role);
        }

        return new ApiResponse(200, $"Role {roleDto.Name} created", roleDto); //fix a strange System.Text.Json exception shown only in Debug_SSB
    }
    public async Task<ApiResponse> UpdateRoleAsync(RoleDto roleDto)
    {
        var response = new ApiResponse(200, $"Role {roleDto.Name} updated", roleDto);

        if (!_roleManager.Roles.Any(r => r.Name == roleDto.Name))
            response = new ApiResponse(400, $"The role {roleDto.Name} doesn't exist. Role name cannot be updated.");
        else
        {
            if (roleDto.Name == DefaultRoleNames.Administrator)
                response = new ApiResponse(403, $"Role {roleDto.Name} cannot be edited");
            else
            {
                // Create the permissions
                var role = await _roleManager.FindByNameAsync(roleDto.Name);

                var claims = await _roleManager.GetClaimsAsync(role);
                var permissions = claims.OrderBy(x => x.Value).Where(x => x.Type == ApplicationClaimTypes.Permission).Select(x => x.Value).ToList();

                foreach (var permission in permissions)
                {
                    await _roleManager.RemoveClaimAsync(role, new Claim(ApplicationClaimTypes.Permission, permission));
                }

                foreach (var claim in roleDto.Permissions)
                {
                    var result = await _roleManager.AddClaimAsync(role, new Claim(ApplicationClaimTypes.Permission, _entityPermissions.GetPermissionByName(claim)));

                    if (!result.Succeeded)
                        await _roleManager.DeleteAsync(role);
                }
            }
        }

        return response;
    }
    public async Task<ApiResponse> DeleteRoleAsync(string name)
    {
        var response = new ApiResponse(200, $"Role {name} deleted successfully");

        // Check if the role is used by a user
        var users = await _userManager.GetUsersInRoleAsync(name);
        if (users.Any())
            response = new ApiResponse(404, $"Role {name} Is Used by users.");
        else
        {
            if (name == DefaultRoleNames.Administrator)
                response = new ApiResponse(403, $"Role {name} is Administrator");
            else
            {
                // Delete the role
                var role = await _roleManager.FindByNameAsync(name);
                await _roleManager.DeleteAsync(role);
            }
        }

        return response;
    }
    #endregion
}