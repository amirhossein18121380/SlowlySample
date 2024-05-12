using Domain.Constants;

namespace Domain.Dto.Language;

public class AddUserLangDto
{
    public Guid UserId { get; set; }
    public int LanguageId { get; set; }
    public LanguageLevel LanguageLevel { get; set; }
    public string? LanguageName { get; set; }
}