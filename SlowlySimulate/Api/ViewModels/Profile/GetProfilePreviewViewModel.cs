using SlowlySimulate.Domain.Models;
using SlowlySimulate.Shared.Dto.Profile;

namespace SlowlySimulate.Api.ViewModels.Profile;

public class GetProfilePreviewViewModel
{
    public string? DisplayName { get; set; }
    public Guid UserId { get; set; }
    public string? SlowlyId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public GenderType GenderType { get; set; }
    public string? Country { get; set; }
    public LetterLength LetterLength { get; set; }
    public ReplyTime ReplyTime { get; set; }
    public string? AboutMe { get; set; }
    public List<string>? Topics { get; set; }
    public List<LanguagePreview>? Languages { get; set; }
}
