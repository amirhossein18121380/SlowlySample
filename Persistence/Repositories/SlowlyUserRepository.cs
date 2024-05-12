using CrossCuttingConcerns.DateTimes;
using Domain.Models;
using Domain.Repositories;

namespace Persistence.Repositories;

public class SlowlyUserRepository : Repository<SlowlyUser, Guid>, ISlowlyUserRepository
{
    public SlowlyUserRepository(SlowlyDbContext dbContext, IDateTimeProvider dateTimeProvider)
        : base(dbContext, dateTimeProvider)
    {
    }

}
