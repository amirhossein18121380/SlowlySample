using Domain.Models;

namespace SlowlySimulate.Api.ViewModels.Profile;
public class ProfileRegisterViewModel
{
    public string DisplayName { get; set; }
    public DateTime BirthDate { get; set; }
    public GenderType GenderType { get; set; }
    public string? Country { get; set; }
    public LetterLength LetterLength { get; set; }
    public ReplyTime ReplyTime { get; set; }
    public string? AboutMe { get; set; }
}
