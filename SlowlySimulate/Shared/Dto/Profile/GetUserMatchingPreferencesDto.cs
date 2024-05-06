namespace SlowlySimulate.Shared.Dto.Profile;

public class GetUserMatchingPreferencesDto
{
    public Guid SlowlyUserId { get; set; }
    public bool AllowAddMeById { get; set; }
    public bool AutoMatch { get; set; }
    public bool ProfileSuggestions { get; set; }
    public bool EnableAgeFilter { get; set; }
    public int? AgeFrom { get; set; }
    public int? AgeTo { get; set; }
    public bool BeMale { get; set; }
    public bool BeFemale { get; set; }
    public bool BeNonBinary { get; set; }
}