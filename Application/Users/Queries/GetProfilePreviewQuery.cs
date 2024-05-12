using Application.Common.Queries;
using Application.Topic.Services;
using Application.Users.Services;
using AutoMapper;
using CrossCuttingConcerns.Caching;
using CrossCuttingConcerns.Models;
using Domain.Dto.Profile;
using Domain.Extension;
using Domain.Identity;
using Domain.Models;
using Domain.Repositories;

namespace Application.Users.Queries;
public class GetProfilePreviewQuery : IQuery<ApiResponse>
{
}

internal class GetProfilePreviewQueryHandler : IQueryHandler<GetProfilePreviewQuery, ApiResponse>
{
    private readonly IUserService _userService;
    private readonly ITopicService _topicService;
    private readonly ICurrentUser _currentUser;
    private readonly IRepository<UserTopic, Guid> _userTopicRepository;
    private readonly IRepository<Domain.Models.UserLanguage, Guid> _userLanguageRepository;
    private readonly IMapper _mapper;
    private readonly ICache _easyCachingProvider;
    public GetProfilePreviewQueryHandler(IUserService userService,
        ITopicService topicService,
        IRepository<UserTopic, Guid> userTopicRepository,
        IRepository<Domain.Models.UserLanguage, Guid> userLanguageRepository,
        ICurrentUser currentUser,
        IMapper mapper, ICache easyCachingProvider)
    {
        _userService = userService;
        _topicService = topicService;
        _userTopicRepository = userTopicRepository;
        _userLanguageRepository = userLanguageRepository;
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
                return new ApiResponse(statusCode: 200, "Success", cachedUserProfile.Value);
            }
        }

        if (_currentUser.UserId == Guid.Empty)
        {
            return new ApiResponse(statusCode: 404, "User Not Found.");
        }
        var user = await _userService.GetByUserId(_currentUser.UserId);

        if (user == null)
        {
            return new ApiResponse(statusCode: 404, "User Not Found.");
        }
        var profilePreview = _mapper.Map<ProfilePreviewDto>(user);

        var topicIdsQuery = _userTopicRepository.GetQueryableSet().Where(x => x.UserId == _currentUser.UserId)
            .Select(x => x.TopicId);
        var topicIds = await _userTopicRepository.ToListAsync(topicIdsQuery);

        var getAllTopics = await _topicService.GetAsync(cancellationToken);
        var userTopics = getAllTopics.Where(x => topicIds.Contains(x.Id)).Select(t => t.Name).ToList();

        var userLanguagesQuery = _userLanguageRepository.GetQueryableSet()
            .Where(x => x.UserId == _currentUser.UserId);

        var userLanguages = await _userLanguageRepository.ToListAsync(userLanguagesQuery);

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