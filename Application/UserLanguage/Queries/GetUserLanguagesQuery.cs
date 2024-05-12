using Application.Common.Queries;
using Application.Decorators.AuditLog;
using Application.Decorators.DatabaseRetry;
using Application.Users.DTOs;
using Domain.Extension;
using Domain.Identity;
using Domain.Repositories;

namespace Application.UserLanguage.Queries;


public class GetUserLanguagesQuery : IQuery<List<LanguagesOfUserDto>>
{
}

[AuditLog]
[DatabaseRetry]
internal class GetTopicsQueryHandler : IQueryHandler<GetUserLanguagesQuery, List<LanguagesOfUserDto>>
{
    private readonly IRepository<Domain.Models.UserLanguage, Guid> _userLanRepository;
    private readonly ICurrentUser _currentUser;
    public GetTopicsQueryHandler(IRepository<Domain.Models.UserLanguage, Guid> userLanRepository,
        ICurrentUser currentUser)
    {
        _userLanRepository = userLanRepository;
        _currentUser = currentUser;
    }

    public async Task<List<LanguagesOfUserDto>> HandleAsync(GetUserLanguagesQuery query, CancellationToken cancellationToken = default)
    {
        var userLanguages = _userLanRepository
            .GetQueryableSet()
            .Where(x => x.UserId == _currentUser.UserId)
            .Select(x => new LanguagesOfUserDto
            {
                LanguageId = x.LanguageId,
                LanguageName = x.LanguageId.GetLanguageName(),
                LanguageLevel = x.LanguageLevel
            });
        return await _userLanRepository.ToListAsync(userLanguages);
    }
}
