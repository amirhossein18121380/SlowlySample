using SlowlySimulate.Application.Common.Commands;
using SlowlySimulate.Domain.Models;
using SlowlySimulate.Domain.Repositories;
using SlowlySimulate.Persistence;

namespace SlowlySimulate.Application.Roles.Commands;

public class DeleteRoleCommand : ICommand<bool>
{
    public Role Role { get; set; }
}

internal class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SlowlyDbContext _context;
    public DeleteRoleCommandHandler(IUnitOfWork unitOfWork, SlowlyDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<bool> HandleAsync(DeleteRoleCommand command, CancellationToken cancellationToken = default)
    {
        _context.Roles.Remove(command.Role);
        var res = await _unitOfWork.SaveChangesAsync(cancellationToken);
        return res == 1;
    }
}
