using Application.Common.Queries;
using AutoMapper;
using CrossCuttingConcerns.Caching;
using CrossCuttingConcerns.Models;
using Domain.Dto.Profile;
using Domain.Identity;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Application.Users.Queries;

public class GetUserProfile : IQuery<ApiResponse>
{
}

internal class GetUserProfileHandler : IQueryHandler<GetUserProfile, ApiResponse>
{
    private readonly ISlowlyUserRepository _slowlyUserRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;
    private readonly IMatchingPreferencesRepository _repository;
    private readonly ICache _easyCachingProvider;
    private readonly ILogger<GetUserProfileHandler> _logger;
    public GetUserProfileHandler(ISlowlyUserRepository slowlyUserRepository, ICurrentUser currentUser, IMapper mapper,
        IMatchingPreferencesRepository repository,
        ICache easyCachingProvider,
        ILogger<GetUserProfileHandler> logger)
    {
        _slowlyUserRepository = slowlyUserRepository;
        _currentUser = currentUser;
        _mapper = mapper;
        _repository = repository;
        _easyCachingProvider = easyCachingProvider;
        _logger = logger;
    }

    public async Task<ApiResponse> HandleAsync(GetUserProfile query, CancellationToken cancellationToken = default)
    {

        var response = new ApiResponse(Status200OK, "Success");
        if (_currentUser.UserId == Guid.Empty)
        {
            response = new ApiResponse(Status404NotFound, "User Not Found.");
        }

        string cacheKey = string.Empty;
        cacheKey = $"UserProfile_{_currentUser.UserId}"; // Customize the cache key
        var cachedUserProfile = await _easyCachingProvider.GetAsync<GetProfileDto>(cacheKey, cancellationToken);

        if (cachedUserProfile.HasValue)
        {
            _logger.LogInformation("UserProfile fetched from Cache successfully.");
            response = new ApiResponse(Status200OK, "Success", cachedUserProfile.Value);
        }

        else
        {
            var user = await _slowlyUserRepository.GetQueryableSet()
                .SingleOrDefaultAsync(x => x.UserId == _currentUser.UserId, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("This User is Not Found.");
                response = new ApiResponse(Status404NotFound, "User Not Found.");
            }

            var profile = _mapper.Map<GetProfileDto>(user);

            var userMatch = await _repository.GetQueryableSet().SingleOrDefaultAsync(x => x.SlowlyUserId == _currentUser.UserId, cancellationToken);
            if (userMatch != null)
            {
                _logger.LogWarning("This User is Not Found.");
                profile.AllowAddMeById = userMatch.AllowAddMeById;
            }

            if (cacheKey != string.Empty)
            {
                _logger.LogInformation("This User cached Successfully.");
                await _easyCachingProvider.SetAsync(cacheKey, profile, TimeSpan.FromMinutes(10), cancellationToken); // Cache for 10 minutes
            }

            response.Result = profile;
        }

        return response;

    }
}
