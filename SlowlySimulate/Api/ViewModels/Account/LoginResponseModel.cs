namespace SlowlySimulate.Api.ViewModels.Account
{
    public class LoginResponseModel
    {
        public bool RequiresTwoFactor { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsNotAllowed { get; set; }
        public bool IsSuccess { get; set; }
    }
}
