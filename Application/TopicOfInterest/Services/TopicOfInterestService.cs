using Application.Common;
using Application.Common.Services;
using Domain.Models;
using Domain.Repositories;

namespace Application.TopicOfInterest.Services;
public class TopicOfInterestService : CrudService<UserTopic>, ITopicOfInterestService
{
    public TopicOfInterestService(IRepository<UserTopic, Guid> topicRepository, Dispatcher dispatcher)
        : base(topicRepository, dispatcher)
    {
    }
}