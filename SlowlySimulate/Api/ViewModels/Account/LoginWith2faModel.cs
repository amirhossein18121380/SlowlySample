using System.ComponentModel.DataAnnotations;

namespace SlowlySimulate.Api.ViewModels.Account
{
    public class LoginWith2faModel : AccountFormModel
    {
        [Display(Name = "RememberBrowser")]
        public bool RememberMachine { get; set; }
    }
}
