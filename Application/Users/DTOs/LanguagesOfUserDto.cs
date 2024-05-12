using Domain.Constants;

namespace Application.Users.DTOs;
public class LanguagesOfUserDto
{
    public int LanguageId { get; set; }
    public string LanguageName { get; set; }
    public LanguageLevel LanguageLevel { get; set; }
}