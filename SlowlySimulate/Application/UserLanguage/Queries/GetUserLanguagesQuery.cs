using SlowlySimulate.Api.ViewModels.Profile;
using SlowlySimulate.Application.Common.Queries;
using SlowlySimulate.Application.Decorators.AuditLog;
using SlowlySimulate.Application.Decorators.DatabaseRetry;
using SlowlySimulate.Domain.Identity;
using SlowlySimulate.Domain.Repositories;
using SlowlySimulate.Domain.Repositories.Extension;

namespace SlowlySimulate.Application.UserLanguage.Queries;


public class GetUserLanguagesQuery : IQuery<List<LanguagesOfUser>>
{
}

[AuditLog]
[DatabaseRetry]
internal class GetTopicsQueryHandler : IQueryHandler<GetUserLanguagesQuery, List<LanguagesOfUser>>
{
    private readonly IRepository<Domain.Models.UserLanguage, Guid> _userLanRepository;
    private readonly ICurrentUser _currentUser;
    public GetTopicsQueryHandler(IRepository<Domain.Models.UserLanguage, Guid> userLanRepository,
        ICurrentUser currentUser)
    {
        _userLanRepository = userLanRepository;
        _currentUser = currentUser;
    }

    public async Task<List<LanguagesOfUser>> HandleAsync(GetUserLanguagesQuery query, CancellationToken cancellationToken = default)
    {
        var userLanguages = _userLanRepository
            .GetQueryableSet()
            .Where(x => x.UserId == _currentUser.UserId)
            .Select(x => new LanguagesOfUser
            {
                LanguageId = x.LanguageId,
                LanguageName = x.LanguageId.GetLanguageName(),
                LanguageLevel = x.LanguageLevel
            });
        return await _userLanRepository.ToListAsync(userLanguages);
    }
}
