using Application.Common;
using Application.Common.Services;
using Domain.Repositories;

namespace Application.Topic.Services;
public class TopicService : CrudService<Domain.Models.Topic>, ITopicService
{
    public TopicService(IRepository<Domain.Models.Topic, Guid> topicRepository, Dispatcher dispatcher)
        : base(topicRepository, dispatcher)
    {
    }
}