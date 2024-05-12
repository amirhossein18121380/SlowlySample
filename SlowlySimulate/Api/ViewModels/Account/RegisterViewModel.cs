using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace SlowlySimulate.Api.ViewModels.Account
{
    public class RegisterViewModel : LoginInputModel
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string PasswordConfirm { get; set; }


        //profile section 
        public string DisplayName { get; set; }
        public DateTime BirthDate { get; set; }
        public GenderType GenderType { get; set; }
        public string? Country { get; set; }
        public LetterLength LetterLength { get; set; }
        public ReplyTime ReplyTime { get; set; }
        public string? AboutMe { get; set; }
    }
}
