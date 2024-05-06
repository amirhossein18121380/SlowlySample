using AutoMapper;
using EasyCaching.Core;
using Microsoft.EntityFrameworkCore;
using SlowlySimulate.Application.Common.Queries;
using SlowlySimulate.CrossCuttingConcerns.Models;
using SlowlySimulate.Domain.Identity;
using SlowlySimulate.Domain.Repositories.Extension;
using SlowlySimulate.Persistence;
using SlowlySimulate.Shared.Dto.Profile;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace SlowlySimulate.Application.Users.Queries;
public class GetProfilePreviewQuery : IQuery<ApiResponse>
{
}

internal class GetProfilePreviewQueryHandler : IQueryHandler<GetProfilePreviewQuery, ApiResponse>
{
    private readonly SlowlyDbContext _dbContext;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;
    private readonly IEasyCachingProvider _easyCachingProvider;
    public GetProfilePreviewQueryHandler(SlowlyDbContext userRepository, ICurrentUser currentUser,
        IMapper mapper, IEasyCachingProvider easyCachingProvider)
    {
        _dbContext = userRepository;
        _currentUser = currentUser;
        _mapper = mapper;
        _easyCachingProvider = easyCachingProvider;
    }

    public async Task<ApiResponse> HandleAsync(GetProfilePreviewQuery query, CancellationToken cancellationToken = default)
    {
        string cacheKey = string.Empty;
        if (_currentUser.UserId != Guid.Empty)
        {
            cacheKey = $"ProfilePreview_{_currentUser.UserId}";
            var cachedUserProfile = await _easyCachingProvider.GetAsync<ProfilePreviewDto>(cacheKey, cancellationToken);

            if (cachedUserProfile.HasValue)
            {
                return new ApiResponse(Status200OK, "Success", cachedUserProfile.Value);
            }
        }


        var user = await _dbContext.SlowlyUsers
            .SingleOrDefaultAsync(x => x.UserId == _currentUser.UserId, cancellationToken: cancellationToken);
        if (user == null)
        {
            return new ApiResponse(statusCode: 404, "User Not Found.");
        }
        var profilePreview = _mapper.Map<ProfilePreviewDto>(user);

        var topicIds = await _dbContext.UserTopics.Where(x => x.UserId == _currentUser.UserId)
            .Select(x => x.TopicId).ToListAsync(cancellationToken);
        var userTopics = await _dbContext.Topics.Where(x => topicIds.Contains(x.Id)).Select(t => t.Name).ToListAsync(cancellationToken);

        var userLanguages = await _dbContext.UserLanguages
            .Where(x => x.UserId == _currentUser.UserId).ToListAsync(cancellationToken);

        var languagePreview = new List<LanguagePreview>();
        foreach (var x in userLanguages)
        {
            languagePreview.Add(new LanguagePreview()
            {
                Id = x.LanguageId,
                Name = x.LanguageId.GetLanguageName(),
                Level = x.LanguageLevel
            });

        }

        profilePreview.Topics = userTopics;
        profilePreview.Languages = languagePreview;

        if (cacheKey != string.Empty)
        {
            await _easyCachingProvider.SetAsync(cacheKey, profilePreview, TimeSpan.FromMinutes(10), cancellationToken); // Cache for 10 minutes
        }

        return new ApiResponse(statusCode: 200, "Success", profilePreview);
    }
}