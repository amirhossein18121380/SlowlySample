using Microsoft.EntityFrameworkCore;
using SlowlySimulate.Application.Common.Commands;
using SlowlySimulate.CrossCuttingConcerns.Models;
using SlowlySimulate.Domain.Models;
using SlowlySimulate.Domain.Repositories;
using SlowlySimulate.Shared.Resources;
using SlowlySimulate.Shared.Resources.User;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace SlowlySimulate.Application.Users.Commands;

public class UpdateProfileCommand : ICommand<ApiResponse>
{
    public Guid UserId { get; set; }
    public LetterLength LetterLength { get; set; }
    public ReplyTime ReplyTime { get; set; }
    public string? AboutMe { get; set; }
    public bool AllowAddMeById { get; set; }
}

internal class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand, ApiResponse>
{
    private readonly ISlowlyUserRepository _slowlyUserRepository;
    private readonly IMatchingPreferencesRepository _repository;

    public UpdateProfileCommandHandler(ISlowlyUserRepository userRepository, IMatchingPreferencesRepository repository)
    {
        _slowlyUserRepository = userRepository;
        _repository = repository;

    }

    public async Task<ApiResponse> HandleAsync(UpdateProfileCommand command, CancellationToken cancellationToken = default)
    {
        var user = _slowlyUserRepository.GetQueryableSet().SingleOrDefault(x => x.UserId == command.UserId);
        if (user is null)
        {
            return new ApiResponse(Status404NotFound, "Not Found");
        }

        user.LetterLength = command.LetterLength;
        user.ReplyTime = command.ReplyTime;
        user.AboutMe = command.AboutMe;

        var userMatch = await _repository.GetQueryableSet().SingleOrDefaultAsync(x => x.SlowlyUserId == command.UserId, cancellationToken);
        if (userMatch is null)
        {
            //return new ApiResponse(Status404NotFound, "Not Found");
            return new ApiResponse(Status404NotFound, ApiResponses.Not_Found);
        }
        userMatch.AllowAddMeById = command.AllowAddMeById;

        await _repository.UpdateAsync(userMatch, cancellationToken);
        await _slowlyUserRepository.UpdateAsync(user, cancellationToken);

        var res = await _slowlyUserRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return res == 2 ? new ApiResponse(Status200OK, UserR.Success, user) :
            new ApiResponse(Status409Conflict, UserR.Cannot_Update);
    }

}

