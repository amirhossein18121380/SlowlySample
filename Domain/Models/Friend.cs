namespace Domain.Models
{
    public class Friend : Entity<Guid>, IAggregateRoot
    {
        public Guid RequestedById { get; set; }
        public Guid RequestedToId { get; set; }
        public bool IsStarredUser { get; set; } = false;
        public bool IsHiddenUser { get; set; } = false;
        public bool IsRemovedUser { get; set; } = false;
        public bool IsReportedUser { get; set; } = false;
        public DateTime RequestTime { get; set; } = DateTime.UtcNow;
        public FriendRequestFlag FriendRequestFlag { get; set; } = FriendRequestFlag.None;
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
