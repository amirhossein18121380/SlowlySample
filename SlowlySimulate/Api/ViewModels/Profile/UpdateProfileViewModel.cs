namespace SlowlySimulate.Api.ViewModels.Profile
{
    public class UpdateProfileViewModel
    {
        public int LetterLength { get; set; }
        public int ReplyTime { get; set; }
        public string? AboutMe { get; set; }
        public bool AllowAddMeById { get; set; }
    }
}
