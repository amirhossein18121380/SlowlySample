using Application.Common.Commands;
using Application.Common.Services;
using CrossCuttingConcerns.Models;
using Domain.Repositories;

namespace Application.UserLanguage.Commands;

public class DeleteUserLanguageCommand : ICommand<ApiResponse>
{
    public Guid UserId { get; set; }
    public int LanguageId { get; set; }
}

internal class DeleteUserLanguageCommandHandler : ICommandHandler<DeleteUserLanguageCommand, ApiResponse>
{
    private readonly ICrudService<Domain.Models.UserLanguage> _userLanguageService;
    private readonly IRepository<Domain.Models.UserLanguage, Guid> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserLanguageCommandHandler(ICrudService<Domain.Models.UserLanguage> userLanguageService, IUnitOfWork unitOfWork,
        IRepository<Domain.Models.UserLanguage, Guid> repository)
    {
        _userLanguageService = userLanguageService;
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task<ApiResponse> HandleAsync(DeleteUserLanguageCommand command, CancellationToken cancellationToken = default)
    {
        var lang = _repository.GetQueryableSet().SingleOrDefault(x => x.LanguageId == command.LanguageId & x.UserId == command.UserId);
        if (lang == null)
        {
            return new ApiResponse(404, "Not Found");
        }

        using (await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken))
        {
            await _userLanguageService.DeleteAsync(lang, cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }

        return new ApiResponse(statusCode: 200, "Success");
    }
}