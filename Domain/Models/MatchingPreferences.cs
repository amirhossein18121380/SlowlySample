namespace Domain.Models;
public class MatchingPreferences : Entity<Guid>, IAggregateRoot
{
    public Guid SlowlyUserId { get; set; }
    public bool AllowAddMeById { get; set; } = false;
    public bool AutoMatch { get; set; } = true;
    public bool ProfileSuggestions { get; set; } = true;
    public bool EnableAgeFilter { get; set; } = false;
    public int? AgeFrom { get; set; }
    public int? AgeTo { get; set; }
    public bool BeMale { get; set; } = true;
    public bool BeFemale { get; set; } = true;
    public bool BeNonBinary { get; set; } = true;
}