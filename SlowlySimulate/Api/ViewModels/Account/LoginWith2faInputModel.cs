using System.ComponentModel.DataAnnotations;

namespace SlowlySimulate.Api.ViewModels.Account
{
    public class LoginWith2faInputModel : LoginWith2faModel
    {
        [DataType(DataType.Text)]
        public string TwoFactorCode { get; set; }
    }
}
