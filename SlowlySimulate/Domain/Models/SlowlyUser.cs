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