namespace SlowlySimulate.Domain.Models;

public class SlowlyUser : Entity<Guid>, IAggregateRoot
{
    public Guid UserId { get; set; }
    public string SlowlyId { get; set; }
    public string? DisplayName { get; set; }
    public string? AboutMe { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Country { get; set; }
    public GenderType Gender { get; set; }
    public LetterLength LetterLength { get; set; } = LetterLength.NoPreferences;
    public ReplyTime ReplyTime { get; set; } = ReplyTime.AsSoonAsPossible;
    public string? Language { get; set; }
    public DateTime CreatedAt { get; set; }
    public TimeSpan LastOnline { get; set; }
    public DateTime JoinedDate { get; set; }
    public bool DarkMode { get; set; }
    public bool PushNotification { get; set; }
    public bool ConfirmBeforeSendingTheLetter { get; set; }
    public string AppLang { get; set; } = "en";

    public ICollection<Letter> SentLetters { get; set; }
    public ICollection<Letter> ReceivedLetters { get; set; }
    public ICollection<Interest> Interests { get; set; }
    public ICollection<Stamp> Stamps { get; set; }
    public ICollection<Topic> Topics { get; set; }
    public List<UserLanguage> UserLanguages { get; set; } = new List<UserLanguage>();

    public virtual ICollection<Friend> SentFriendRequests { get; set; } // represents friend requests sent by the user.
    public virtual ICollection<Friend> ReceivedFriendRequests { get; set; } // represents friend requests received by the user.
    public virtual ICollection<Friend> Friends { get; set; } // represents the actual friends (approved connections).

    public ApplicationUser ApplicationUser { get; set; }


    public void SetRandomSlowlyId()
    {
        Random random = new Random();
        const string allowedCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        SlowlyId = new string(Enumerable.Repeat(allowedCharacters, 6)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}

public enum GenderType
{
    Man,
    Woman
}
public enum LetterLength
{
    NoPreferences,
    Short,
    ShortMedium,
    Medium,
    MediumLong,
    Long,
}

public enum ReplyTime
{
    AsSoonAsPossible,
    WithinAWeek,
    Within2Weeks,
    Within3Weeks,
    WithinAMonth,
    OverAMonth
}