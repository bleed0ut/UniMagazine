using System.ComponentModel.DataAnnotations;

namespace UniMagazine.Models
{
    public class ResetPasswordVM
    {
        [Required]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password cannot be empty !!")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,100}$", ErrorMessage = "The {0} must contain at least one letter, one number, and one special character.")]
        [DataType(DataType.Password)]
        public string? Password { set; get; }


        [Required(ErrorMessage = "Confirm password cannot be empty !!")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string? ConfirmPassword { set; get; }
        
        [Required]
        public string? Code { get; set; }
    }
}
