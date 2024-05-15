namespace Domain.Models;

public class Letter : Entity<Guid>, IAggregateRoot
{
    public Guid SenderId { get; set; }
    public Guid RecipientId { get; set; }
    public required string Content { get; set; }
    public DateTime SentTime { get; set; }
    public DateTime DeliveryTime { get; set; }
    public bool IsRead { get; set; }

    public SlowlyUser? Sender { get; set; }
    public SlowlyUser? Recipient { get; set; }
}