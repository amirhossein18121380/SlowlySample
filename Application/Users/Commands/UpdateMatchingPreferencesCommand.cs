﻿using Application.Common.Commands;
using CrossCuttingConcerns.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands;

public class UpdateMatchingPreferencesCommand : ICommand<ApiResponse>
{
    public Guid UserId { get; set; }
    public bool AllowAddMeById { get; set; }
    public bool AutoMatch { get; set; }
    public bool ProfileSuggestions { get; set; }
    public bool EnableAgeFilter { get; set; }
    public bool BeMale { get; set; }
    public bool BeFemale { get; set; }
    public bool BeNonBinary { get; set; }
    public int? AgeFrom { get; set; }
    public int? AgeTo { get; set; }
}

internal class UpdateMatchingPreferencesCommandHandler : ICommandHandler<UpdateMatchingPreferencesCommand, ApiResponse>
{
    private readonly IMatchingPreferencesRepository _repository;

    public UpdateMatchingPreferencesCommandHandler(IMatchingPreferencesRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse> HandleAsync(UpdateMatchingPreferencesCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _repository.GetQueryableSet().SingleOrDefaultAsync(x => x.SlowlyUserId == command.UserId, cancellationToken);
        if (user is null)
        {
            return new ApiResponse(statusCode: 404, "Not Found");
        }

        user.AllowAddMeById = command.AllowAddMeById;
        user.AutoMatch = command.AutoMatch;
        user.ProfileSuggestions = command.ProfileSuggestions;
        user.BeMale = command.BeMale;
        user.BeFemale = command.BeFemale;
        user.BeNonBinary = command.BeNonBinary;
        user.EnableAgeFilter = command.EnableAgeFilter;
        if (command.EnableAgeFilter)
        {
            user.AgeFrom = command.AgeFrom;
            user.AgeTo = command.AgeTo;
        }
        else
        {
            user.AgeFrom = default;
            user.AgeTo = default;
        }

        await _repository.UpdateAsync(user, cancellationToken);
        var res = await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return res == 1 ? new ApiResponse(200, "Success", user) :
            new ApiResponse(409, "Cannot Update.");
    }
}

