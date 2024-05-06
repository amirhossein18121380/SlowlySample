using System.ComponentModel.DataAnnotations;

namespace SlowlySimulate.Api.ViewModels.Account
{
    public class UpdatePasswordViewModel
    {
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        public string NewPasswordConfirm { get; set; }

        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
    }
}
