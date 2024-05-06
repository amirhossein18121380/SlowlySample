using System.ComponentModel.DataAnnotations;

namespace SlowlySimulate.Api.ViewModels.Account
{
    public class LoginWithRecoveryCodeInputModel : LoginWith2faModel
    {
        [DataType(DataType.Text)]
        public string RecoveryCode { get; set; }
    }
}
