using SlowlySimulate.Application.Common.Services;
using SlowlySimulate.Domain.Models;

namespace SlowlySimulate.Application.Common.Commands;

public class AddOrUpdateEntityCommand<TEntity, TResult> : ICommand<TResult>
    where TEntity : Entity<Guid>, IAggregateRoot
{
    public AddOrUpdateEntityCommand(TEntity entity)
    {
        Entity = entity;
    }

    public TEntity Entity { get; set; }
}

internal class AddOrUpdateEntityCommandHandler<TEntity, TResult> : ICommandHandler<AddOrUpdateEntityCommand<TEntity, TResult>, TResult>
where TEntity : Entity<Guid>, IAggregateRoot
{
    private readonly ICrudService<TEntity> _crudService;

    public AddOrUpdateEntityCommandHandler(ICrudService<TEntity> crudService)
    {
        _crudService = crudService;
    }

    public async Task<TResult> HandleAsync(AddOrUpdateEntityCommand<TEntity, TResult> command, CancellationToken cancellationToken = default)
    {
        await _crudService.AddOrUpdateAsync(command.Entity);
        return default(TResult);
    }
}
