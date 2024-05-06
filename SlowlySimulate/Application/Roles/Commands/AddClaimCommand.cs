using Microsoft.AspNetCore.Identity;
using SlowlySimulate.Application.Common.Commands;
using SlowlySimulate.CrossCuttingConcerns.Models;
using SlowlySimulate.Domain.Models;
using SlowlySimulate.Domain.Repositories;

namespace SlowlySimulate.Application.Roles.Commands;

public class AddClaimCommand : ICommand<ApiResponse>
{
    public ApplicationRole Role { get; set; }
    public RoleClaim Claim { get; set; }
}

internal class AddClaimCommandHandler : ICommandHandler<AddClaimCommand, ApiResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    public AddClaimCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse> HandleAsync(AddClaimCommand command, CancellationToken cancellationToken = default)
    {
        command.Role.Claims.Add(command.Claim);
        var res = await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new ApiResponse(statusCode: 200);
    }
}

public class RoleClaim : IdentityRoleClaim<Guid> { }