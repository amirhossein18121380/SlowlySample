using SlowlySimulate.Application.Common;
using SlowlySimulate.Application.Common.Services;
using SlowlySimulate.Domain.Models;
using SlowlySimulate.Domain.Repositories;

namespace SlowlySimulate.Application.TopicOfInterest.Services;
public class TopicOfInterestService : CrudService<UserTopic>, ITopicOfInterestService
{
    public TopicOfInterestService(IRepository<UserTopic, Guid> topicRepository, Dispatcher dispatcher)
        : base(topicRepository, dispatcher)
    {
    }
}