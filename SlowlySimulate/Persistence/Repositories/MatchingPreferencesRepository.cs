using SlowlySimulate.CrossCuttingConcerns.DateTimes;
using SlowlySimulate.Domain.Models;
using SlowlySimulate.Domain.Repositories;

namespace SlowlySimulate.Persistence.Repositories;
public class MatchingPreferencesRepository : Repository<MatchingPreferences, Guid>, IMatchingPreferencesRepository
{
    public MatchingPreferencesRepository(SlowlyDbContext dbContext, IDateTimeProvider dateTimeProvider)
        : base(dbContext, dateTimeProvider)
    {
    }

}
