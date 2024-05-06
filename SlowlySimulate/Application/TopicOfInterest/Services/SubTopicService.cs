using SlowlySimulate.Application.Common;
using SlowlySimulate.Application.Common.Services;
using SlowlySimulate.Domain.Models;
using SlowlySimulate.Domain.Repositories;

namespace SlowlySimulate.Application.TopicOfInterest.Services;
public class SubTopicService : CrudService<SubTopic>, ISubTopicService
{
    public SubTopicService(IRepository<SubTopic, Guid> subTopicRepository, Dispatcher dispatcher)
        : base(subTopicRepository, dispatcher)
    {
    }
}