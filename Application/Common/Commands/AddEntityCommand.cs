using Application.Common.Services;
using Domain.Models;

namespace Application.Common.Commands;

public class AddEntityCommand<TEntity, TResult> : ICommand<TResult>
    where TEntity : Entity<Guid>, IAggregateRoot
{
    public AddEntityCommand(TEntity entity)
    {
        Entity = entity;
    }

    public TEntity Entity { get; set; }
}

internal class AddEntityCommandHandler<TEntity, TResult> : ICommandHandler<AddEntityCommand<TEntity, TResult>, TResult>
where TEntity : Entity<Guid>, IAggregateRoot
{
    private readonly ICrudService<TEntity> _crudService;

    public AddEntityCommandHandler(ICrudService<TEntity> crudService)
    {
        _crudService = crudService;
    }

    public async Task<TResult> HandleAsync(AddEntityCommand<TEntity, TResult> command, CancellationToken cancellationToken = default)
    {
        await _crudService.AddAsync(command.Entity);
        return default(TResult);
    }
}
