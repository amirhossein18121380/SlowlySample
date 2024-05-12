using Application.Common.Commands;
using CrossCuttingConcerns.Models;
using Domain.Models;
using Domain.Repositories;
using Domain.Resources;
using Domain.Resources.User;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands;

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
            return new ApiResponse(statusCode: 404, "Not Found");
        }

        user.LetterLength = command.LetterLength;
        user.ReplyTime = command.ReplyTime;
        user.AboutMe = command.AboutMe;

        var userMatch = await _repository.GetQueryableSet().SingleOrDefaultAsync(x => x.SlowlyUserId == command.UserId, cancellationToken);
        if (userMatch is null)
        {
            return new ApiResponse(statusCode: 404, ApiResponses.Not_Found);
        }
        userMatch.AllowAddMeById = command.AllowAddMeById;

        await _repository.UpdateAsync(userMatch, cancellationToken);
        await _slowlyUserRepository.UpdateAsync(user, cancellationToken);

        var res = await _slowlyUserRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return res == 2 ? new ApiResponse(200, UserR.Success, user) :
            new ApiResponse(statusCode: 409, UserR.Cannot_Update);
    }

}

