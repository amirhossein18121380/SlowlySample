using Microsoft.EntityFrameworkCore;
using SlowlySimulate.Application.Common.Commands;
using SlowlySimulate.Application.TopicOfInterest.Services;
using SlowlySimulate.CrossCuttingConcerns.Models;
using SlowlySimulate.Domain.Models;
using SlowlySimulate.Domain.Repositories;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace SlowlySimulate.Application.TopicOfInterest.Commands;

public class AddSubTopicCommand : ICommand<ApiResponse>
{
    public Guid UserId { get; set; }
    public Guid TopicId { get; set; }
    public required string SubTopicName { get; set; }
}

internal class AddSubTopicCommandHandler : ICommandHandler<AddSubTopicCommand, ApiResponse>
{
    private readonly IRepository<Domain.Models.SubTopic, Guid> _subTopicRepository;
    private readonly IRepository<UserTopic, Guid> _useTopicRepository;
    private readonly ISubTopicService _subTopicService;
    public AddSubTopicCommandHandler(ISubTopicService subTopicService,
        IRepository<Domain.Models.SubTopic, Guid> subTopicRepository,
        IRepository<UserTopic, Guid> uTRepository)
    {
        _subTopicService = subTopicService;
        _subTopicRepository = subTopicRepository;
        _useTopicRepository = uTRepository;
    }

    public async Task<ApiResponse> HandleAsync(AddSubTopicCommand command, CancellationToken cancellationToken = default)
    {
        var userTopics = await _useTopicRepository
            .GetQueryableSet()
            .SingleOrDefaultAsync(x => x.UserId == command.UserId & x.TopicId == command.TopicId,
                cancellationToken);

        if (userTopics?.TopicId == Guid.Empty)
        {
            return new ApiResponse(Status404NotFound, "TopicId Not Found");
        }

        var query = _subTopicRepository.GetQueryableSet()
            .Where(x => x.Name.Equals(command.SubTopicName));
        var exist = await _subTopicRepository.ToListAsync(query);

        if (exist.Any())
        {
            return new ApiResponse(Status400BadRequest, "This SubTopic Already Exist.");
        }

        var subTopic = new SubTopic()
        {
            TopicId = command.TopicId,
            Name = command.SubTopicName,
        };

        await _subTopicService.AddAsync(subTopic, cancellationToken);
        return new ApiResponse(statusCode: 200, "Success");
    }
}