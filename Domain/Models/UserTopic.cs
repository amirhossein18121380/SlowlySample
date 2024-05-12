namespace Domain.Models;


public class UserTopic : Entity<Guid>, IAggregateRoot
{
    public Guid UserId { get; set; }
    public Guid TopicId { get; set; }
    public bool? IsExcluded { get; set; }
    public bool? IsInterest { get; set; }
}