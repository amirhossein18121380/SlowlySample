using Application.Common.Services;
using Domain.Models;

namespace Application.Common.Commands;

public class UpdateEntityCommand<TEntity, TResult> : ICommand<TResult>
    where TEntity : Entity<Guid>, IAggregateRoot
{
    public UpdateEntityCommand(TEntity entity)
    {
        Entity = entity;
    }

    public TEntity Entity { get; set; }
}

internal class UpdateEntityCommandHandler<TEntity, TResult> : ICommandHandler<UpdateEntityCommand<TEntity, TResult>, TResult>
where TEntity : Entity<Guid>, IAggregateRoot
{
    private readonly ICrudService<TEntity> _crudService;

    public UpdateEntityCommandHandler(ICrudService<TEntity> crudService)
    {
        _crudService = crudService;
    }

    public async Task<TResult> HandleAsync(UpdateEntityCommand<TEntity, TResult> command, CancellationToken cancellationToken = default)
    {
        await _crudService.UpdateAsync(command.Entity);
        return default(TResult);
    }
}
