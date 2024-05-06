using SlowlySimulate.CrossCuttingConcerns.Models;
using SlowlySimulate.Shared.Dto.Role;

namespace SlowlySimulate.Application.Roles.Service;

public interface IRoleService
{
    Task<ApiResponse> GetUsers(int pageSize = 10, int pageNumber = 0);
    ApiResponse GetPermissions();
    Task<ApiResponse> GetRolesAsync(int pageSize = 0, int pageNumber = 0);
    Task<ApiResponse> GetRoleAsync(string roleName);
    Task<ApiResponse> CreateRoleAsync(RoleDto roleDto);
    Task<ApiResponse> UpdateRoleAsync(RoleDto roleDto);
    Task<ApiResponse> DeleteRoleAsync(string name);
    Task<ApiResponse> GetRolesPermissionsAsync(string roleName);
    Task<ApiResponse> GetRolePermissionAsync(string roleName);
}