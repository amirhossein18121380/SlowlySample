using System.ComponentModel.DataAnnotations;

namespace SlowlySimulate.Api.ViewModels.Account
{
    public class AuthenticatorVerificationCodeViewModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "VerificationCode")]
        public string Code { get; set; }
    }
}
