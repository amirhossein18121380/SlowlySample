using Application.Common.Commands;
using Domain.Models;
using Domain.Repositories;

namespace Application.Users.Commands;

public class DeleteUserCommand : ICommand<bool>
{
    public SlowlyUser User { get; set; }
}

internal class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, bool>
{
    private readonly ISlowlyUserRepository _userRepository;

    public DeleteUserCommandHandler(ISlowlyUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> HandleAsync(DeleteUserCommand command, CancellationToken cancellationToken = default)
    {
        _userRepository.Delete(command.User);
        var deleted = await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return deleted == 1;
    }
}
