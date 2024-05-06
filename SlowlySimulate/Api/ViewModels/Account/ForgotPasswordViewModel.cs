using System.ComponentModel.DataAnnotations;

namespace SlowlySimulate.Api.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
