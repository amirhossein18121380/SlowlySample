using Application.Common.Queries;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries;

public class GetUserQuery : IQuery<SlowlyUser>
{
    public Guid Id { get; set; }
}

internal class GetUserQueryHandler : IQueryHandler<GetUserQuery, SlowlyUser>
{
    private readonly ISlowlyUserRepository _slowlyUserRepository;

    public GetUserQueryHandler(ISlowlyUserRepository slowlyUserRepository)
    {
        _slowlyUserRepository = slowlyUserRepository;
    }

    public async Task<SlowlyUser> HandleAsync(GetUserQuery query, CancellationToken cancellationToken = default)
    {
        return await _slowlyUserRepository.GetQueryableSet().FirstOrDefaultAsync(x => x.UserId == query.Id, cancellationToken: cancellationToken);
    }
}
