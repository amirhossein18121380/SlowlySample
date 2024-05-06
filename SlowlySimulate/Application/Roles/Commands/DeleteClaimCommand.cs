using SlowlySimulate.Application.Common.Commands;
using SlowlySimulate.Domain.Models;
using SlowlySimulate.Domain.Repositories;

namespace SlowlySimulate.Application.Roles.Commands;

public class DeleteClaimCommand : ICommand<bool>
{
    public ApplicationRole Role { get; set; }
    public RoleClaim Claim { get; set; }
}

internal class DeleteClaimCommandHandler : ICommandHandler<DeleteClaimCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    public DeleteClaimCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> HandleAsync(DeleteClaimCommand command, CancellationToken cancellationToken = default)
    {
        command.Role.Claims.Remove(command.Claim);
        var res = await _unitOfWork.SaveChangesAsync(cancellationToken);
        return res == 1;
    }
}
