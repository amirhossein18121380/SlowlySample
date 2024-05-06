namespace SlowlySimulate.Api.ViewModels.Profile;

public class TopicViewModel
{
    public List<Domain.Models.Topic> FixedTopics { get; set; }
    public List<Domain.Models.Topic> UserTopics { get; set; }
}

public class ExcludeTopicViewModel
{
    public List<Domain.Models.Topic> FixedTopics { get; set; }
    public List<Domain.Models.Topic> UserTopics { get; set; }
    public List<Domain.Models.Topic> UserExcludedTopics { get; set; }
}