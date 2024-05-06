using SlowlySimulate.Domain.Constants;

namespace SlowlySimulate.Api.ViewModels.Profile;

public class LanguagesOfUser
{
    public int LanguageId { get; set; }
    public string LanguageName { get; set; }
    public LanguageLevel LanguageLevel { get; set; }
}