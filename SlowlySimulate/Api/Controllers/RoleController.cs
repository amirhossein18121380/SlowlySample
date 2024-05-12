using Application.Roles.Service;
using CrossCuttingConcerns.Models;
using Domain.Dto.Role;
using Domain.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlowlySimulate.Api.ViewModels.Role;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace SlowlySimulate.Api.Controllers;

public class RoleController : Controller
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    [Authorize(Permissions.Role.Read)]
    public ApiResponse GetPermissions()
    {
        try
        {
            var response = _roleService.GetPermissions();
            return new ApiResponse(response.StatusCode, response.Message, response.Result);
        }
        catch (Exception ex)
        {
            return new ApiResponse(Status500InternalServerError, ex.Message, ex.Data);
        }

    }

    [HttpGet]
    [Authorize(Permissions.Role.Read)]
    public async Task<ActionResult<ApiResponse>> GetRolePermission(GetRolePermissionViewModel model)
    {
        try
        {
            var response = await _roleService.GetRolesPermissionsAsync(model.RoleName);
            return new ApiResponse(response.StatusCode, response.Message, response.Result);
        }
        catch (Exception ex)
        {
            return new ApiResponse(Status500InternalServerError, ex.Message, ex.Data);
        }
    }

    [HttpGet]
    [Authorize(Permissions.Role.Read)]
    public async Task<ApiResponse> GetRoleDto(GetRolePermissionViewModel model)
    {
        try
        {
            var response = await _roleService.GetRolePermissionAsync(model.RoleName);
            return new ApiResponse(response.StatusCode, response.Message, response.Result);
        }
        catch (Exception ex)
        {
            return new ApiResponse(Status500InternalServerError, ex.Message, ex.Data);
        }
    }

    [HttpGet]
    [Authorize(Permissions.Role.Read)]
    public async Task<ApiResponse> GetRoles()
    {
        try
        {
            var response = await _roleService.GetRolesAsync();
            return new ApiResponse(response.StatusCode, response.Message, response.Result);
        }
        catch (Exception ex)
        {
            return new ApiResponse(Status500InternalServerError, ex.Message, ex.Data);
        }
    }

    [HttpGet]
    [Authorize(Permissions.Role.Read)]
    public async Task<ActionResult<ApiResponse>> Index()
    {
        try
        {
            var response = await _roleService.GetRolesAsync(10, 0);

            if (response.IsSuccessStatusCode)
            {
                var roles = response.Result;
                return View(roles);
            }
            return View(new List<RoleDto>());
        }
        catch (Exception ex)
        {
            return new ApiResponse(Status500InternalServerError, ex.Message, ex.Data);
        }

    }

    [HttpPost]
    [Authorize(Permissions.Role.Create)]
    public async Task<ActionResult<ApiResponse>> CreateRole([FromBody] RoleDto roleDto)
    {
        try
        {
            var response = await _roleService.CreateRoleAsync(roleDto);
            return new ApiResponse(response.StatusCode, response.Message);
        }
        catch (Exception ex)
        {
            return new ApiResponse(Status500InternalServerError, ex.Message, ex.Data);
        }
    }

    [HttpDelete]
    [Authorize(Permissions.Role.Delete)]
    public async Task<ActionResult<ApiResponse>> DeleteRole([FromBody] RoleDeleteRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
            {
                return new ApiResponse(Status400BadRequest, "Invalid request. Role name is required.");
            }

            var response = await _roleService.DeleteRoleAsync(request.Name);
            return new ApiResponse(response.StatusCode, response.Message);
        }
        catch (Exception ex)
        {
            return new ApiResponse(Status500InternalServerError, ex.Message, ex.Data);
        }
    }

    [HttpPut]
    [Authorize(Permissions.Role.Update)]
    public async Task<ActionResult<ApiResponse>> UpdateRoleAsync([FromBody] RoleDto roleDto)
    {
        try
        {
            var response = await _roleService.UpdateRoleAsync(roleDto);
            return new ApiResponse(response.StatusCode, response.Message);
        }
        catch (Exception ex)
        {
            return new ApiResponse(Status500InternalServerError, ex.Message, ex.Data);
        }
    }
}