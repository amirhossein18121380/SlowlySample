using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SlowlySimulate.Application.Common.Queries;
using SlowlySimulate.CrossCuttingConcerns.Models;
using SlowlySimulate.Domain.Identity;
using SlowlySimulate.Domain.Repositories;
using SlowlySimulate.Shared.Dto.Profile;

namespace SlowlySimulate.Application.Users.Queries;

public class GetUserMatchingPreferencesQuery : IQuery<ApiResponse>
{
}

internal class GetUserMatchingPreferencesQueryHandler : IQueryHandler<GetUserMatchingPreferencesQuery, ApiResponse>
{
    private readonly IMatchingPreferencesRepository _repository;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;
    public GetUserMatchingPreferencesQueryHandler(IMatchingPreferencesRepository repository, ICurrentUser currentUser, IMapper mapper)
    {
        _repository = repository;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<ApiResponse> HandleAsync(GetUserMatchingPreferencesQuery query, CancellationToken cancellationToken = default)
    {
        var user = await _repository.GetQueryableSet()
            .SingleOrDefaultAsync(x => x.SlowlyUserId == _currentUser.UserId, cancellationToken: cancellationToken);
        if (user == null)
        {
            return new ApiResponse(statusCode: 404, "User Not Found.");
        }

        var matching = _mapper.Map<GetUserMatchingPreferencesDto>(user);

        return new ApiResponse(statusCode: 200, "Success", matching);
    }
}