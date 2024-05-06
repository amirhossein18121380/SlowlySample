using SlowlySimulate.Application.Common.Commands;
using SlowlySimulate.Application.Common.Services;
using SlowlySimulate.CrossCuttingConcerns.Models;
using SlowlySimulate.Domain.Repositories;
using SlowlySimulate.Domain.Repositories.Extension;
using SlowlySimulate.Shared.Dto.Language;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace SlowlySimulate.Application.UserLanguage.Commands;

public class UpdateUserLanguageLevelCommand : ICommand<ApiResponse>
{
    public Guid UserId { get; set; }
    public int LanguageId { get; set; }
    public int LanguageLevel { get; set; }
}

internal class UpdateUserLanguageLevelCommandHandler : ICommandHandler<UpdateUserLanguageLevelCommand, ApiResponse>
{
    private readonly ICrudService<Domain.Models.UserLanguage> _userLanguageService;
    private readonly IRepository<Domain.Models.UserLanguage, Guid> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserLanguageLevelCommandHandler(ICrudService<Domain.Models.UserLanguage> userLanguageService, IUnitOfWork unitOfWork,
        IRepository<Domain.Models.UserLanguage, Guid> repository)
    {
        _userLanguageService = userLanguageService;
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task<ApiResponse> HandleAsync(UpdateUserLanguageLevelCommand command, CancellationToken cancellationToken = default)
    {
        var ul = _repository.GetQueryableSet().SingleOrDefault(x => x.LanguageId == command.LanguageId & x.UserId == command.UserId);
        if (ul == null)
        {
            return new ApiResponse(Status404NotFound, "Not Found");
        }

        ul.LanguageLevel = command.LanguageLevel.GetLanguageLevel();

        using (await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken))
        {
            await _userLanguageService.UpdateAsync(ul, cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }


        var updateModel = new UpdateLanguageDto()
        {
            UserLanguage = ul,
            LanguageName = ul.LanguageId.GetLanguageName()
        };

        return new ApiResponse(statusCode: 200, "Success", updateModel);
    }
}

