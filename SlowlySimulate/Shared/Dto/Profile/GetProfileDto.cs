using SlowlySimulate.Domain.Models;

namespace SlowlySimulate.Shared.Dto.Profile
{
    public class GetProfileDto
    {
        public string? DisplayName { get; set; }
        public Guid UserId { get; set; }
        public string? SlowlyId { get; set; }
        public DateTime? BirthDate { get; set; }
        public GenderType GenderType { get; set; }
        public string? Country { get; set; }
        public LetterLength LetterLength { get; set; }
        public ReplyTime ReplyTime { get; set; }
        public string? AboutMe { get; set; }
        public bool AllowAddMeById { get; set; }
    }
}
