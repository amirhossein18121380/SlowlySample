using SlowlySimulate.Domain.Constants;

namespace SlowlySimulate.Shared.Dto.Language;

public class AddUserLangDto
{
    public Guid UserId { get; set; }
    public int LanguageId { get; set; }
    public LanguageLevel LanguageLevel { get; set; }
    public string? LanguageName { get; set; }
}