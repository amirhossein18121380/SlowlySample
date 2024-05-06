using System.ComponentModel.DataAnnotations;

namespace SlowlySimulate.Api.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        public string UserId { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
