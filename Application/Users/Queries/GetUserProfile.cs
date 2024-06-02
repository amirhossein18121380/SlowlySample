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

        //not good solution
        try
        {
            var cachedUserProfileTask = _easyCachingProvider.GetAsync<GetProfileDto>(cacheKey, cancellationToken);

            if (await Task.WhenAny(cachedUserProfileTask, Task.Delay(TimeSpan.FromSeconds(1))) == cachedUserProfileTask)
            {
                var cachedUserProfile = await cachedUserProfileTask;

                if (cachedUserProfile.HasValue)
                {
                    _logger.LogInformation("UserProfile fetched from Cache successfully.");
                    response = new ApiResponse(Status200OK, "Success", cachedUserProfile.Value);
                }
            }
            else
            {
                // Handle the case where retrieval from cache takes more than 2 seconds
                _logger.LogWarning("UserProfile retrieval from cache timed out.");
                // You can choose to return a default response or take other actions here
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user profile from cache.");
        }



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


        //not good solution
        try
        {
            if (cacheKey != string.Empty)
            {
                //await _easyCachingProvider.SetAsync(cacheKey, profile, TimeSpan.FromMinutes(10), cancellationToken);
                var setProfile = _easyCachingProvider.SetAsync(cacheKey, profile, TimeSpan.FromMinutes(10), cancellationToken); // Cache for 10 minutes

                if (await Task.WhenAny(setProfile, Task.Delay(TimeSpan.FromSeconds(1))) == setProfile)
                {
                    await setProfile;
                    _logger.LogInformation("This User cached Successfully.");
                }
                else
                {
                    // Handle the case where retrieval from cache takes more than 2 seconds
                    _logger.LogWarning("UserProfile set from cache timed out.");
                    // You can choose to return a default response or take other actions here
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting user profile in cache.");
        }

        response.Result = profile;

        return response;

    }
}
