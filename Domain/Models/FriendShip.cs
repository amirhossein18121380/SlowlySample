namespace Domain.Models
{
    public class FriendShip : Entity<Guid>, IAggregateRoot
    {
        public Guid RequestedById { get; set; } //Foreign key referencing the first user in the friendship.
        public Guid RequestedToId { get; set; } //Foreign key referencing the second user in the friendship.
        public bool IsStarredUser { get; set; } = false;
        public bool IsHiddenUser { get; set; } = false;
        public bool IsRemovedUser { get; set; } = false;
        public bool IsReportedUser { get; set; } = false;
        public DateTime RequestTime { get; set; } = DateTime.UtcNow;
        public FriendRequestFlag FriendRequestFlag { get; set; } = FriendRequestFlag.None;
        public SlowlyUser? RequestedByUser { get; set; }
        public SlowlyUser? RequestedToUser { get; set; }
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
