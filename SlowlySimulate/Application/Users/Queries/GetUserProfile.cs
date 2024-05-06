using AutoMapper;
using EasyCaching.Core;
using Microsoft.EntityFrameworkCore;
using SlowlySimulate.Application.Common.Queries;
using SlowlySimulate.CrossCuttingConcerns.Models;
using SlowlySimulate.Domain.Identity;
using SlowlySimulate.Domain.Repositories;
using SlowlySimulate.Persistence;
using SlowlySimulate.Shared.Dto.Profile;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace SlowlySimulate.Application.Users.Queries;

public class GetUserProfile : IQuery<ApiResponse>
{
}

internal class GetUserProfileHandler : IQueryHandler<GetUserProfile, ApiResponse>
{
    private readonly SlowlyDbContext _dbContext;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;
    private readonly IMatchingPreferencesRepository _repository;
    private readonly IEasyCachingProvider _easyCachingProvider;
    private readonly ILogger<GetUserProfileHandler> _logger;
    public GetUserProfileHandler(SlowlyDbContext userRepository, ICurrentUser currentUser, IMapper mapper,
        IMatchingPreferencesRepository repository,
        IEasyCachingProvider easyCachingProvider,
        ILogger<GetUserProfileHandler> logger)
    {
        _dbContext = userRepository;
        _currentUser = currentUser;
        _mapper = mapper;
        _repository = repository;
        _easyCachingProvider = easyCachingProvider;
        _logger = logger;
    }

    public async Task<ApiResponse> HandleAsync(GetUserProfile query, CancellationToken cancellationToken = default)
    {
        string cacheKey = string.Empty;
        if (_currentUser.UserId != Guid.Empty)
        {
            cacheKey = $"UserProfile_{_currentUser.UserId}"; // Customize the cache key
            var cachedUserProfile = await _easyCachingProvider.GetAsync<GetProfileDto>(cacheKey, cancellationToken);

            if (cachedUserProfile.HasValue)
            {
                _logger.LogInformation("UserProfile fetched from Cache successfully.");
                return new ApiResponse(Status200OK, "Success", cachedUserProfile.Value);
            }
        }


        var user = await _dbContext.SlowlyUsers.SingleOrDefaultAsync(x => x.UserId == _currentUser.UserId, cancellationToken);
        if (user == null)
        {
            _logger.LogWarning("This User is Not Found.");
            return new ApiResponse(Status404NotFound, "User Not Found.");
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

        return new ApiResponse(Status200OK, "Success", profile);
    }
}
