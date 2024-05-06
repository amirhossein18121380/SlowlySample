using SlowlySimulate.CrossCuttingConcerns.DateTimes;
using SlowlySimulate.Domain.Models;
using SlowlySimulate.Domain.Repositories;

namespace SlowlySimulate.Persistence.Repositories;

public class SlowlyUserRepository : Repository<SlowlyUser, Guid>, ISlowlyUserRepository
{
    public SlowlyUserRepository(SlowlyDbContext dbContext, IDateTimeProvider dateTimeProvider)
        : base(dbContext, dateTimeProvider)
    {
    }

}
