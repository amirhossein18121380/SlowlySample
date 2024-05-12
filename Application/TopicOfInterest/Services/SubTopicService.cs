using Application.Common;
using Application.Common.Services;
using Domain.Models;
using Domain.Repositories;

namespace Application.TopicOfInterest.Services;
public class SubTopicService : CrudService<SubTopic>, ISubTopicService
{
    public SubTopicService(IRepository<SubTopic, Guid> subTopicRepository, Dispatcher dispatcher)
        : base(subTopicRepository, dispatcher)
    {
    }
}