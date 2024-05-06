namespace SlowlySimulate.Domain.Models
{
    public class Friend : Entity<Guid>, IAggregateRoot
    {
        public Guid RequestedById { get; set; }
        public SlowlyUser RequestedBy { get; set; } // User who initiated the request
        public Guid RequestedToId { get; set; }
        public SlowlyUser RequestedTo { get; set; } // User who received the request
        public bool IsStarredUser { get; set; }
        public bool IsHiddenUser { get; set; }
        public bool IsRemovedUser { get; set; }
        public bool IsReportedUser { get; set; }
        public DateTime? RequestTime { get; set; }
        public FriendRequestFlag FriendRequestFlag { get; set; }
    }

    public enum FriendRequestFlag
    {
        None, // Default state (pending)
        Approved,
        Rejected,
        Blocked,
        Spam
    }

}
