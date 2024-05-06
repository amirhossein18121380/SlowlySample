using Microsoft.CodeAnalysis;
using SlowlySimulate.Application.Common.Commands;
using SlowlySimulate.Application.Common.Services;
using SlowlySimulate.CrossCuttingConcerns.Models;
using SlowlySimulate.Domain.Repositories;
using SlowlySimulate.Domain.Repositories.Extension;
using SlowlySimulate.Shared.Dto.Language;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace SlowlySimulate.Application.UserLanguage.Commands;

public class AddUserLanguageCommand : ICommand<ApiResponse>
{
    public Guid UserId { get; set; }
    public int LanguageId { get; set; }
    public int LanguageLevel { get; set; }
}

internal class AddUserLanguageCommandHandler : ICommandHandler<AddUserLanguageCommand, ApiResponse>
{
    private readonly ICrudService<Domain.Models.UserLanguage> _userLanguageService;
    private readonly IRepository<Domain.Models.UserLanguage, Guid> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddUserLanguageCommandHandler(ICrudService<Domain.Models.UserLanguage> userLanguageService, IUnitOfWork unitOfWork,
        IRepository<Domain.Models.UserLanguage, Guid> repository)
    {
        _userLanguageService = userLanguageService;
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task<ApiResponse> HandleAsync(AddUserLanguageCommand command, CancellationToken cancellationToken = default)
    {
        var lanExist = _repository.GetQueryableSet().Any(x => x.LanguageId == command.LanguageId & x.UserId == command.UserId);
        if (lanExist)
        {
            return new ApiResponse(Status409Conflict, "AlreadyExist");
        }

        if (!command.LanguageId.IsValidLanguageId())
        {
            return new ApiResponse(Status400BadRequest, "Invalid Language");
        }

        var userLan = new Domain.Models.UserLanguage
        {
            UserId = command.UserId,
            LanguageId = command.LanguageId,
            LanguageLevel = command.LanguageLevel.GetLanguageLevel()
        };


        using (await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken))
        {
            await _userLanguageService.AddAsync(userLan, cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }

        var returnModel = new AddUserLangDto
        {
            UserId = userLan.UserId,
            LanguageId = command.LanguageId,
            LanguageLevel = userLan.LanguageLevel,
            LanguageName = command.LanguageId.GetLanguageName()
        };

        return new ApiResponse(statusCode: 200, "Success", returnModel);
    }
}
