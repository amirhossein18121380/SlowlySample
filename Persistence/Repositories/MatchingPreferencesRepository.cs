using CrossCuttingConcerns.DateTimes;
using Domain.Models;
using Domain.Repositories;

namespace Persistence.Repositories;
public class MatchingPreferencesRepository : Repository<MatchingPreferences, Guid>, IMatchingPreferencesRepository
{
    public MatchingPreferencesRepository(SlowlyDbContext dbContext, IDateTimeProvider dateTimeProvider)
        : base(dbContext, dateTimeProvider)
    {
    }

}
