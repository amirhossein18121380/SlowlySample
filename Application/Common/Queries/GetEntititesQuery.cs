using Domain.Models;
using Domain.Repositories;

namespace Application.Common.Queries;

public class GetEntititesQuery<TEntity> : IQuery<List<TEntity>>
     where TEntity : Entity<Guid>, IAggregateRoot
{
}

internal class GetEntititesQueryHandler<TEntity> : IQueryHandler<GetEntititesQuery<TEntity>, List<TEntity>>
where TEntity : Entity<Guid>, IAggregateRoot
{
    private readonly IRepository<TEntity, Guid> _repository;

    public GetEntititesQueryHandler(IRepository<TEntity, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<List<TEntity>> HandleAsync(GetEntititesQuery<TEntity> query, CancellationToken cancellationToken = default)
    {
        return await _repository.ToListAsync(_repository.GetQueryableSet());
    }
}
