using SlowlySimulate.Application.Common;
using SlowlySimulate.Application.Common.Services;
using SlowlySimulate.Domain.Repositories;

namespace SlowlySimulate.Application.Topic.Services;
public class TopicService : CrudService<Domain.Models.Topic>, ITopicService
{
    public TopicService(IRepository<Domain.Models.Topic, Guid> topicRepository, Dispatcher dispatcher)
        : base(topicRepository, dispatcher)
    {
    }
}