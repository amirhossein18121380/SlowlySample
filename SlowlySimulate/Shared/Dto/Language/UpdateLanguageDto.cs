namespace SlowlySimulate.Shared.Dto.Language;
public class UpdateLanguageDto
{
    public Domain.Models.UserLanguage UserLanguage { get; set; }
    public string? LanguageName { get; set; }
}