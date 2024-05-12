using Application.Common.Commands;
using Application.Roles.Service;
using CrossCuttingConcerns.Models;
using Domain.Models;
using Domain.Repositories;

namespace Application.Roles.Commands;

public class DeleteRoleCommand : ICommand<ApiResponse>
{
    public required Role Role { get; set; }
}

internal class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, ApiResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoleService _roleService;
    public DeleteRoleCommandHandler(IUnitOfWork unitOfWork, IRoleService roleService)
    {
        _unitOfWork = unitOfWork;
        _roleService = roleService;
    }

    public async Task<ApiResponse> HandleAsync(DeleteRoleCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Role.Name == null)
        {
            return new ApiResponse(404, "Not Found");
        }
        var response = await _roleService.DeleteRoleAsync(command.Role.Name);
        return response;
    }
}
