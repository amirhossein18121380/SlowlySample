using SlowlySimulate.Domain.Constants;

namespace SlowlySimulate.Shared.Dto.Profile;

public class LanguagePreview
{
    public int Id { get; set; }
    public string Name { get; set; }
    public LanguageLevel Level { get; set; }
}