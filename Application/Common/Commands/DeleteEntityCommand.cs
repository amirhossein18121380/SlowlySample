using Application.Common.Services;
using Domain.Models;

namespace Application.Common.Commands;

public class DeleteEntityCommand<TEntity, TResult> : ICommand<TResult>
     where TEntity : Entity<Guid>, IAggregateRoot
{
    public TEntity Entity { get; set; }
}

internal class DeleteEntityCommandHandler<TEntity, TResult> : ICommandHandler<DeleteEntityCommand<TEntity, TResult>, TResult>
where TEntity : Entity<Guid>, IAggregateRoot
{
    private readonly ICrudService<TEntity> _crudService;

    public DeleteEntityCommandHandler(ICrudService<TEntity> crudService)
    {
        _crudService = crudService;
    }

    public async Task<TResult> HandleAsync(DeleteEntityCommand<TEntity, TResult> command, CancellationToken cancellationToken = default)
    {
        await _crudService.DeleteAsync(command.Entity);
        return default(TResult);
    }
}
