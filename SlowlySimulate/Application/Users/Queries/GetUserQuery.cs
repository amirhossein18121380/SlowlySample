using Microsoft.EntityFrameworkCore;
using SlowlySimulate.Application.Common.Queries;
using SlowlySimulate.Domain.Models;
using SlowlySimulate.Persistence;

namespace SlowlySimulate.Application.Users.Queries;

public class GetUserQuery : IQuery<SlowlyUser>
{
    public Guid Id { get; set; }
}

internal class GetUserQueryHandler : IQueryHandler<GetUserQuery, SlowlyUser>
{
    private readonly SlowlyDbContext _dbContext;

    public GetUserQueryHandler(SlowlyDbContext userRepository)
    {
        _dbContext = userRepository;
    }

    public async Task<SlowlyUser> HandleAsync(GetUserQuery query, CancellationToken cancellationToken = default)
    {
        return await _dbContext.SlowlyUsers.FirstOrDefaultAsync(x => x.UserId == query.Id, cancellationToken: cancellationToken);
    }
}
